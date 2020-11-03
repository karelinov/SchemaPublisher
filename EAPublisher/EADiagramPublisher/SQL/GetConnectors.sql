SELECT
	t_connector.connector_id,
	t_connector.name,
	t_connector.direction,
	t_connector.notes,
	t_connector.start_object_id,
	t_connector.end_object_id,
	t_object_source.Name AS object_source_name,
	t_object_source.Object_Type AS object_source_type,
	t_object_end.Name AS object_end_name,
	t_object_end.Object_Type AS object_end_type
FROM
((t_connector as t_connector
INNER JOIN t_object as t_object_source ON t_object_source.Object_ID = t_connector.Start_Object_ID)
INNER JOIN t_object as t_object_end ON t_object_end.Object_ID = t_connector.End_Object_ID)
WHERE t_object_source.Package_ID IN (#PARAM0#)
or t_object_end.Package_ID IN (#PARAM0#)
