SELECT 
 t_connectortag.elementid,
 t_connectortag.property,
 t_connectortag.value

FROM t_connectortag
WHERE t_connectortag.ElementID IN
(
SELECT
	t_connector.Connector_ID
FROM
((t_connector as t_connector
INNER JOIN t_object as t_object_source ON t_object_source.Object_ID = t_connector.Start_Object_ID)
INNER JOIN t_object as t_object_end ON t_object_end.Object_ID = t_connector.End_Object_ID)
WHERE 
 t_object_source.Package_ID IN (#PARAM0#)
 or t_object_end.Package_ID IN (#PARAM0#)
)