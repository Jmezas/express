    
alter proc LOG_BusquedaProducto--  1,1,0,'polo'
@IdEmpresa int =0   ,
@idSucursal int = 0,
@idAlmacen int = 0,
@vFiltro Varchar(30)=''
as        
begin        
 select t1.iIdMat As Id, t1.sCodMaterial As CodMaterial, t1.sNomMaterial, t5.sCodigoSunat  , t4.iIdAlm, t4.IdSucursal
 from LOG_Material t1  	
	Inner join LOG_StockAlmacenS t2
		On t1.iIdMat = t2.IdMat
	Inner join LOG_Almacen t4
		On t2.IdAlmacen = t4.iIdAlm
	Inner join log_unidadMedida t5 
		On t1.iIdUM = t5.iIdUnidad  
  Where t1.IdEmpresa = @IdEmpresa   
	And t2.IdSucursal = @idSucursal
	And ((@idAlmacen>0 And t4.iIdAlm = @idAlmacen) Or (@idAlmacen=0)) 
	And ((len(@vFiltro)>0 And (t1.sCodMaterial like '%' + @vFiltro +'%' Or t1.sNomMaterial like '%' + @vFiltro +'%') ) Or (Len(@vFiltro)=0))
	-- And t1.sCodMaterial like '%' + @vFiltro +'%' Or t1.sNomMaterial like '%' + @vFiltro +'%' 
	And t1.sEstado ='A'    

end

select * from LOG_Almacen where IdSucursal = 1