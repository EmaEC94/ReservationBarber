IF OBJECT_ID('spbReservationtoHourAvailble') IS NULL
    EXEC('create PROCEDURE spbReservationtoHourAvailble AS SET NOCOUNT ON;')
GO

-- Procedimiento actualizado para obtener las horas disponibles
ALTER PROCEDURE spbReservationtoHourAvailble
 @Day date,
 @UserId int
AS
BEGIN
	-- Convertir @Day a datetime2 para poder agregar horas y minutos
	DECLARE @DayWithTime AS datetime2 = CAST(@Day AS datetime2);
	-- Agregar 8 horas 
	DECLARE @DayAvailable AS datetime2 = DATEADD(MINUTE, 8*60, @DayWithTime);
    -- Declaramos un rango de horas disponibles en un solo día
    WITH HourRange AS (
        -- Generamos un rango de horas (de 08:00 AM a 08:00 PM)
        SELECT DATEADD(MINUTE, number * 60, @DayAvailable) AS AvailableHour
        FROM master.dbo.spt_values
        WHERE type = 'P' AND number <= 12  -- 12 intervalos de 60 minutos
    )
    
    -- Seleccionamos las horas que no están ocupadas por citas
    SELECT AvailableHour
    FROM HourRange
    WHERE CONVERT(time, AvailableHour) NOT IN (
        -- Verificamos las horas ocupadas en el día y para el usuario específico
        SELECT DISTINCT CONVERT(time, Apointment)  -- Convertimos Apointment a tipo time
        FROM dbo.Reservations
        WHERE CAST(Apointment AS date) = @Day  -- Filtramos por el día
        AND userBarberId = @UserId  -- Filtramos por el usuario
    )
    ORDER BY AvailableHour;  -- Ordenamos las horas disponibles
END
GO

/*
DECLARE @Day AS date = '2024-12-28';  -- Día para obtener las horas disponibles
DECLARE @UserId AS int = 1;  -- ID del barbero/usuario

EXEC spbReservationtoHourAvailble @Day, @UserId;
*/