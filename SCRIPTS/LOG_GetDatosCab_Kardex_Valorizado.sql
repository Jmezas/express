Create PROC LOG_GetDatosCab_Kardex_Valorizado 1
@idEmpresa int = 0
AS
BEGIN
  SELECT 
		--cabecera uno
		'APELLIDOS Y NOMBRES, DENOMINACI�N O RAZ�N SOCIAL:'  as Nombre,
		'ESTABLECIMIENTO (1):' as Establecimiento,
		'C�DIGO DE LA EXISTENCIA:' as Codigo,
		'TIPO (TABLA 5):' as Tipo,
		'DESCRIPCI�N:' as Descripcion,
		'C�DIGO DE LA UNIDAD DE MEDIDA (TABLA 6):' as CodigoUnidad,
		'M�TODO DE VALUACI�N:' MetodoEvaluacion,

		--cabecera 2
		sRUC as RUC, sRazonSocialE as RazonSocial, '01 MERCADERIA' as Tipo, '07' as UM, 'PROMEDIO' as MetodoEvaluacion,
		 'FORMATO 13.1: "REGISTRO DE INVENTARIO PERMANENTE VALORIZADO - DETALLE DEL INVENTARIO VALORIZADO"' as Formato,
		'PER�ODO:' as Periodo,
		'RUC:' as TituloRuc,

		--cabecera 3
		'DOCUMENTO DE TRASLADO, COMPROBANTE DE PAGO, DOCUMENTO INTERNO O SIMILAR' as nomDocumento,
		'TIPO DE OPERACI�N'as TipoOperacion,
		'ENTRADAS' as Entrada,
		'SALIDAS'as Salidas,
		'SALDO FINAL' SaldoFinal,
		'FECHA' as Fecha,
		'TIPO (TABLA 10)' as Tipo,
		'SERIE' as Serie,
		'N�MERO' as Numero,
		'(TABLA 12)' as Tabla,
		'CANTIDAD'as Cantidad,
		'COSTO UNITARIO' as CostoUnitario,
		'COSTO TOTAL' as CostoTotal
				
  FROM GEN_Empresa Where iID_Empresa = @idEmpresa
END