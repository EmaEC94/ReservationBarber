CREATE OR ALTER PROCEDURE spbReservationtoHourAvailble
    @Day DATE,
    @UserId INT,
    @DurationMinutes INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DayStart DATETIME2 = DATEADD(HOUR, 8, CAST(@Day AS DATETIME2));  -- 08:00
    DECLARE @DayEnd   DATETIME2 = DATEADD(HOUR, 20, CAST(@Day AS DATETIME2)); -- 20:00

    ------------------------------------------------------------------------------------
    -- 1. DÍA COMPLETO BLOQUEADO
    ------------------------------------------------------------------------------------
    IF EXISTS (
        SELECT 1
        FROM dbo.UserException
        WHERE UserBarberId = @UserId
          AND IsFullDay = 1
          AND State = 1
          AND CAST(StartDate AS DATE) = @Day
    )
    BEGIN
        SELECT CAST(NULL AS DATETIME2) AS AvailableHour WHERE 1 = 0;
        RETURN;
    END

    ------------------------------------------------------------------------------------
    -- 2. GENERAR HORAS (intervalos de 15 min)
    ------------------------------------------------------------------------------------
    ;WITH HourRange AS (
        SELECT 
            DATEADD(MINUTE, v.number * 15, @DayStart) AS AvailableHour,
            DATEADD(MINUTE, v.number * 15 + @DurationMinutes, @DayStart) AS EndHour
        FROM master.dbo.spt_values v
        WHERE v.type = 'P'
          AND DATEADD(MINUTE, v.number * 15 + @DurationMinutes, @DayStart) <= @DayEnd
    )

    ------------------------------------------------------------------------------------
    -- 3. HORAS DISPONIBLES (SIN SOLAPES REALES)
    ------------------------------------------------------------------------------------
    SELECT hr.AvailableHour
    FROM HourRange hr
    WHERE NOT EXISTS (
        ----------------------------------------------------------------------
        -- RESERVAS
        ----------------------------------------------------------------------
        SELECT 1
        FROM dbo.Reservations r
        WHERE r.UserBarberId = @UserId
          AND CAST(r.StartDate AS DATE) = @Day   -- 👈 FILTRO CLAVE
          AND r.State = 1
          AND r.StartDate < hr.EndHour
          AND r.EndDate   > hr.AvailableHour
    )
    AND NOT EXISTS (
        ----------------------------------------------------------------------
        -- EXCEPCIONES PARCIALES
        ----------------------------------------------------------------------
        SELECT 1
        FROM dbo.UserException ue
        WHERE ue.UserBarberId = @UserId
          AND ue.State = 1
          AND ue.IsFullDay = 0
          AND CAST(ue.StartDate AS DATE) = @Day
          AND ue.StartDate < hr.EndHour
          AND ue.EndDate   > hr.AvailableHour
    )
    ORDER BY hr.AvailableHour;
END
GO
