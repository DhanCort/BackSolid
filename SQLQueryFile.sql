use Odyssey2DB

create database Odyssey2DB

drop table dbo.ElementValue
drop table dbo.Job;
drop table Calculation;
drop table dbo.Category;
drop table dbo.Printshop
drop table dbo.Attribute
drop table dbo.Element
drop table dbo.ElementTemplate
drop table dbo.ElementTemplateElementTemplate
drop table dbo.PrintshopElementTemplate
drop table dbo.AttributeElementTemplate
drop table dbo.Enumeration
drop table dbo.GroupCalculation
drop table dbo.ResourceClassification
drop table dbo.Value
drop table dbo.Ascendants
drop table dbo.__EFMigrationsHistory
SELECT name FROM SYS.TABLES order by name;



use Odyssey2DB
GO
EXEC sp_changedbowner 'sa' 
GO




delete from Calculation;
delete from dbo.Printshop
delete from dbo.Attribute
delete from dbo.ElementTemplate
delete from dbo.ElementTemplateElementTemplate
delete from dbo.AttributeElementTemplate
delete from dbo.Enumeration
delete from dbo.Element


select * from ElementType where ResOrPro = 'Process'



select * from enumeration
select * from elementTemplateelementtemplate
select * from Printshop;
delete from Printshop;

select * from Attribute order by CustomName
where Pk in (
select PkAttribute from AttributeElementTemplate where PkElementTemplate = 4)

select * from AttributeElementTemplate;

select * from elementtype;

select * from printshop;

select * from calculation;

select * from calculation;

delete from calculation where Pk = 11 or Pk=13;


select * from dbo.Calculation
select * from Element join ElementTemplate on Element.



select 




delete from AttributeElementTemplate where PkElementTemplate = 152;

delete from calculation

select * from ElementTemplate;
select * from AttributeElementTemplate
where PkElementTemplate = 4
select * from Attribute

--TRAER ATRIBUTOS
select c.Pk, XJDFTemplateId, XJDFName, Datatype
from [dbo].[Attribute] as a, [dbo].[AttributeElementTemplate] as b, [dbo].[ElementTemplate] as c
where a.Pk = b.PkAttribute and b.PKElementTemplate = c.Pk and
c.XJDFTemplateId = 'MediaIntent'

select XJDFTemplateId, XJDFName, Datatype
from [dbo].[Attribute] as a, [dbo].[ElementTemplateElementTemplate] as b, [dbo].[ElementTemplate] as c
where a.Pk = b.PkAttribute and b.PKElementTemplate = c.Pk and
c.XJDFTemplateId = 'ColorIntent'



select * from ElementType where PkPrintshop is not null and classification is not null;
select * from ElementTypeElementType where usage is not null;
select * from inputsandoutputs;
select * from processinworkflow;

delete from inputsandoutputs where Link = 'A';
delete from inputsandoutputs where PkResource = 3;






delete from inputsandoutputs where pk = 15


