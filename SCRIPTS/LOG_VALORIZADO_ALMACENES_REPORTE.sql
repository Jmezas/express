alter PROC [dbo].[LOG_VALORIZADO_ALMACENES_REPORTE] --1,1, '', '', 0, 0,0,1,10  
@IdEmpresa int = 0,  
@IdSucursal int=0,  
@dFecInicio varchar(10)='',  
@dFecFinal varchar(10)='',  
@iIdAlm int = 0,  
@iIdMat int = 0,  
@allReg int=0,   
@numPagina int=0,  
@iCantFilas int=10   
  
AS  
-- Reporte deL Kardex Valorizado:  
BEGIN  
  
-- Poblando tabla #TempBodegaDetValor con la lista de movimientos:  
Select row_number() over (order by convert(varchar(10), t2.dFecReg,103), t1.IdAlmacen, t2.IdMat) as item,  
t1.iTipoMov as Tipo,  t3.sDecripcion, t1.IdAlmacen as iIdAlm, t2.IdMat,   
t1.IdMovimiento as iIdBodCab,  
Case When t1.iTipoMov = 1 Then IsNull(t2.fCantidad,0) Else IsNull(t2.fCantidad,0)*-1 End as Cantidad,  
Case When t1.iTipoMov = 1 Then t2.fCantidad Else 0 End as CantidadEntrada,  
Case When t1.iTipoMov = 1 Then IsNull(t2.fPrecio,0)  Else 0 end as PrecioEntrada,  
Case When t1.iTipoMov = 1 Then 0 Else  IsNull(t2.fCantidad,0) End as CantidadSalida,  
Case When t1.iTipoMov = 1 Then 0 Else  IsNull(t2.fPrecio,0) End as PrecioSalida,  
convert(varchar(10), t2.dFecReg,103) as dfecUsuReg,   
Case When t1.iTipoMov = 1 Then '01' else '31' end as TipoDocumento,   
t1.sSerie as Serie, t1.sNumero as Numero,t4.vte1gen as Operacion, T3.sDecripcion AS TipoOperacion  
into #TempBodegaDetValor  
 from LOG_Movimientos t1  
 Inner join LOG_MovimientoDetalle t2  
  On t1.IdMovimiento = t2.IdMovimiento  
 Inner join LOG_TipoOperacion t3   
  On t3.IdOperacion=t1.IdOperacion  
 inner join GEN_Mgenint t4   
  On t1.iTipomov=t4.ncodite and t4.ncodgen=7   
    
where t1.sEstado ='A'  
 And t2.sEstado ='A'  
 And t1.IdEmpresa = @IdEmpresa  
 And t1.IdSucursal = @IdSucursal  
 And ((len(@dFecInicio)>0 And convert(DateTime, t2.dFecReg,103) >=Convert(DateTime, @dFecInicio,103)) Or (@dFecInicio=''))  
 And ((len(@dFecFinal)>0 And convert(Datetime, t2.dFecReg,103) <= Convert(Datetime, @dFecFinal,103)) Or (@dFecFinal = ''))  
 And ((@iIdAlm <> 0 And  t1.IdAlmacen = @iIdAlm ) Or (@iIdAlm = 0))  
 And ((@iIdMat <> 0 And t2.IdMat = @iIdMat ) Or (@iIdMat = 0))  
  
-- Agrego columnas a la tabla #TempBodegaDetValor:  
alter table #TempBodegaDetValor add TotalStock decimal(20,3) null  
alter table #TempBodegaDetValor add TotalPrecio decimal(20,3) null  
alter table #TempBodegaDetValor add PrecioUnitario decimal(20,3) null  
  
-- Creando índices para la tabla #TempBodegaDetValor:  
create unique index IDX_LOG_BODEGA_01 on #TempBodegaDetValor(item)   
create index IDX_LOG_BODEGA_02 on #TempBodegaDetValor(iIdAlm)   
create index IDX_LOG_BODEGA_03 on #TempBodegaDetValor(IdMat)   
   
-- Poblando tabla #TempBodegaDet con la #TempBodegaDetValor:  
select item, Tipo, iIdAlm, IdMat, Cantidad, PrecioEntrada, PrecioSalida    
 into #TempBodegaDet  
from #TempBodegaDetValor  
  
-- Creando índices para la tabla #TempBodegaDet:  
create index IDX_LOG_BODEGA_04 on #TempBodegaDet(item)  
  
-- Variables:  
declare @Id Int = 0  
declare @iTipoMov Int = 0  
declare @icodAlmacen Int = 0  
declare @icodproducto Int = 0   
declare @ncantidad decimal(10,4) = 0  
declare @nPrecioEntrada Decimal(20,3) = 0  
declare @nPrecioSalida Decimal(20,3)= 0   
declare @nTotalStock  decimal(20,3) = 0   
declare @nTotalPrecio decimal(20,3) = 0   
declare @nPrecioUnitario decimal(20,3) = 0   
declare @PrecioSalida decimal(20,3) = 0  
declare @Inicio Int = 0  
declare @Count Int = 0  
  
-- Total de filas de la tabla #TempBodegaDet:  
set @Count = (select count(item)from #TempBodegaDet)  
  
-- Recorro filas y lleno la #TempBodegaDetValor con los datos de la #TempBodegaDet, a la vez que se realizan los cálculos por cada movimiento:  
set @Inicio = 1  
while @Inicio <= @Count  
 begin  
  -- Capturo valores:  
  select @Id = x.item,   
  @iTipoMov = x.Tipo,   
  @icodAlmacen = x.iIdAlm,   
  @icodproducto = x.IdMat,   
  @ncantidad = x.Cantidad,   
  @nPrecioEntrada = x.PrecioEntrada,  
  @nPrecioSalida = x.PrecioSalida from (Select item, Tipo, iIdAlm, IdMat, Cantidad, PrecioEntrada, PrecioSalida From #TempBodegaDet) x  
  where x.item = @Inicio  
        
  -- Seleccionon el mínimo item de los detalles, respecto del almacén y material:  
  If @Id = (select min(item) From #TempBodegaDetValor where  iIdAlm = @icodAlmacen And IdMat = @icodproducto)  
   begin  
    if @iTipoMov = 1 --Ingreso  
     begin  
      -- Actualizo campos valorizados del detalle:  
      update #TempBodegaDetValor set TotalStock = @ncantidad, TotalPrecio = (@nPrecioEntrada * @ncantidad),   
       PrecioUnitario = case when (@ncantidad > 0) then (@nPrecioEntrada* @ncantidad) / @ncantidad else 0 end  
      From #TempBodegaDetValor where item = @Id    
     End  
    else  
     begin  
      update #TempBodegaDetValor   
      set TotalStock = @ncantidad,   
      TotalPrecio = 0,   
      PrecioUnitario = 0  
      From #TempBodegaDetValor   
      where item = @Id    
     End  
   End  
  else  
   begin          
   -- Obtengo el total stock anterior:  
   declare @TotalStockAnterior decimal(20,2) = (select TotalStock from #TempBodegaDetValor where item = (@Id - 1) and iIdAlm = @icodAlmacen and IdMat = @icodproducto)  
   -- Obtengo la Cantidad de Entrada:  
   declare @CantidadEntrada decimal(20,2) = (select Cantidad from #TempBodegaDetValor where item = @Id and iIdAlm = @icodAlmacen and IdMat = @icodproducto)  
   -- Total de Stcok acumulado:  
   set @nTotalStock = (isnull(@TotalStockAnterior,0) + isnull(@CantidadEntrada,0))          
  
   If @iTipoMov = 1 --ingreso  
    begin  
     If (@nPrecioEntrada * @ncantidad) > 0   
     begin  
      set @nTotalPrecio = (@nPrecioEntrada * @ncantidad) + (select top 1 TotalPrecio from #TempBodegaDetValor   
           where item < @Id  
           and iIdAlm = @icodAlmacen  
           and IdMat = @icodproducto  
           order by item desc)  
  
      set @nPrecioUnitario = Case when (@nTotalStock = 0) then 0 else   
             case When (@nTotalPrecio / @nTotalStock) > 0 then (@nTotalPrecio / @nTotalStock) else (@nTotalPrecio / @nTotalStock) * -1 end    
            End  
  
      -- Actualizo campos valorizados:           
      update #TempBodegaDetValor   
      set   
      TotalStock = @nTotalStock,   
      TotalPrecio = @nTotalPrecio,   
      PrecioUnitario = @nPrecioUnitario            
      from #TempBodegaDetValor   
      where item = @Id  
     End  
                       
      update #TempBodegaDetValor   
      set TotalStock = @nTotalStock,   
      TotalPrecio = @nTotalPrecio,   
      PrecioUnitario = @nPrecioUnitario               
      from #TempBodegaDetValor   
      where item = @Id  
    End  
   else  
    begin  
    If @iTipoMov = 2 --salida  
     begin  
      If @nPrecioSalida = 0  
       begin  
        set @PrecioSalida = (select Top 1 PrecioUnitario from #TempBodegaDetValor   
              where item < @Id   
              and iIdAlm = @icodAlmacen  
              and IdMat = @icodproducto  
              order by item desc)  
  
        update #TempBodegaDetValor set PrecioSalida = @PrecioSalida where item = @Id  
       End  
      else  
       begin  
        set  @PrecioSalida = @nPrecioSalida  
       End                        
  
      -- Obtengo el Total de Precio:  
      set @nTotalPrecio = (@PrecioSalida* @ncantidad) + (select Top 1 TotalPrecio from #TempBodegaDetValor   
                                        where  item < @Id   
                                        and iIdAlm = @icodAlmacen   
                                        and IdMat = @icodproducto   
                                        order by item desc)  
         
      -- Obtengo el Precio Unitario calculado:  
      set @nPrecioUnitario = case When @nTotalStock > 0 then @nTotalPrecio / @nTotalStock else (@PrecioSalida* @ncantidad) / @ncantidad End   
              
      -- Actualizo TotalStock,TotalPrecio y PrecioUnitario:     
      update #TempBodegaDetValor   
      set   
      TotalStock = @nTotalStock,   
      TotalPrecio = @nTotalPrecio,   
      PrecioUnitario = @nPrecioUnitario                 
      from #TempBodegaDetValor   
      where item = @Id  
     End  
    End            
  End  
  
   select @Inicio = @Inicio + 1       
 End  
  
-- Resultado:  
declare @inicio1 int, @fin1 int, @iPaginas Numeric(10,2), @iMenos1 int     
set @iPaginas = convert(numeric(10,2), @iCantFilas)     
set @iMenos1 = @iCantFilas - 1       
  
-- filtro paginado:  
if (@allReg = 0)  
 begin  
    set @inicio1 = ((@numPagina * @iCantFilas) - @iMenos1)      
    set @fin1 = (@numPagina * @iCantFilas)    
 end  
else  
 begin  
    set @inicio1 = 1  
    set @fin1 = 1000000  
 end  
  
begin  
 with table1 as (  
  select Convert(Integer, Count(*) Over (Partition By 0)) Total,  
   row_number() over(order by iIdBodCab ) as item,  
    
      Tipo as TipoMovimiento,   
      dfecUsuReg as Fecha,   
      TipoDocumento as Tipo,   
      Serie +'-'+ Numero as SerieNum,   
	  Serie,
	  Numero,
      TipoOperacion, t1.IdMat,t1.iIdAlm,  
      CantidadEntrada,   
      PrecioEntrada,   
      (CantidadEntrada * PrecioEntrada) as CostoTotalEnt,   
      CantidadSalida,   
      PrecioSalida,   
      (CantidadSalida * PrecioSalida) as CostoTotalSal,   
      case when (TotalStock < 0) then TotalStock *-1 else TotalStock end TotalStock,   
      PrecioUnitario,   
      case when (TotalPrecio < 0) then TotalPrecio *-1 else TotalPrecio end TotalPrecio,  
      t2.sNomMaterial, t3.sAlmacen  
   
  from #TempBodegaDetValor t1  
  Inner Join LOG_Material t2  
   On t1.IdMat = t2.iIdMat  
  Inner Join LOG_Almacen t3  
   On t1.iIdAlm = t3.iIdAlm  
  
)  
select * ,(select ceiling(count(*)/20.00) from table1) as totalPaginas  
  
from table1  
where item >= @inicio1 and item <= @fin1  
  
  
Drop Table #TempBodegaDetValor  
Drop Table #TempBodegaDet   
END   
end  