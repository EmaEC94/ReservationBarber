CREATE OR ALTER PROCEDURE spbReservationtoHourAvailble
 @Day date,
 @UserId int
AS
BEGIN
    DECLARE @DayWithTime datetime2 = CAST(@Day AS datetime2);
    DECLARE @DayAvailable datetime2 = DATEADD(HOUR, 8, @DayWithTime);  -- 08:00 AM

    ------------------------------------------------------------------------------------
    -- 1. VERIFICAR SI EL BARBERO TIENE EL DÍA COMPLETO BLOQUEADO
    ------------------------------------------------------------------------------------
    IF EXISTS (
        SELECT 1 
        FROM dbo.UserException 
        WHERE UserBarberId = @UserId
          AND IsFullDay = 1
          AND State = 1
          AND CAST(StartDate AS date) = @Day
    )
    BEGIN
        -- Si el día está bloqueado, no se devuelve ninguna hora
        SELECT CAST(NULL AS datetime2) AS AvailableHour 
        WHERE 1 = 0;  -- Retorna tabla vacía
        RETURN;
    END

    ------------------------------------------------------------------------------------
    -- 2. GENERAR HORAS DISPONIBLES (08:00 → 20:00)
    ------------------------------------------------------------------------------------
    ;WITH HourRange AS (
        SELECT DATEADD(HOUR, number, @DayAvailable) AS AvailableHour
        FROM master.dbo.spt_values
        WHERE type = 'P'
          AND number BETWEEN 0 AND 11  -- 12 horas
    )

    ------------------------------------------------------------------------------------
    -- 3. SELECCIONAR HORAS NO OCUPADAS POR RESERVAS O BLOQUEOS
    ------------------------------------------------------------------------------------
    SELECT hr.AvailableHour, 
    FROM HourRange hr
    WHERE NOT EXISTS (
        --------------------------------------------------------------------------------
        -- RESERVAS OCUPADAS
        --------------------------------------------------------------------------------
        SELECT 1
        FROM dbo.Reservations r
        WHERE r.userBarberId = @UserId
          AND CAST(r.StartDate AS date) = @Day
          AND CAST(hr.AvailableHour AS time) >= CAST(r.StartDate AS time)
          AND CAST(hr.AvailableHour AS time) <  CAST(r.EndDate AS time)
    )
    AND NOT EXISTS (
        --------------------------------------------------------------------------------
        -- EXCEPCIONES (UserException): RANGOS OCUPADOS
        --------------------------------------------------------------------------------
        SELECT 1
        FROM dbo.UserException ue
        WHERE ue.UserBarberId = @UserId
          AND ue.State = 1              -- Solo activas
          AND ue.IsFullDay = 0          -- Solo rangos parciales
          AND CAST(ue.StartDate AS date) = @Day
          AND CAST(hr.AvailableHour AS time) >= CAST(ue.StartDate AS time)
          AND CAST(hr.AvailableHour AS time) <  CAST(ue.EndDate  AS time)
    )
    ORDER BY hr.AvailableHour;
END
GO


/*
DECLARE @Day AS date = '2025-12-07';  -- Día para obtener las horas disponibles
DECLARE @UserId AS int = 1;  -- ID del userBarberId

EXEC spbReservationtoHourAvailble @Day, @UserId;

daySelected
: 
"2025-12-07"
userId
: 
2
*/