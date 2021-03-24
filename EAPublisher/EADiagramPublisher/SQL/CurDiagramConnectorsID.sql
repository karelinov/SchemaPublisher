select 
 t_connector.connector_id,
 t_diagramlinks.hidden
from t_connector
left outer join t_diagramlinks on(t_diagramlinks.connectorid = t_connector.connector_id)
where 
t_diagramlinks.diagramid = (select diagram_ID from t_diagram where ea_Guid="#PARAM0#")
and
t_connector.start_object_id in
(
select object_id
from 
 t_diagramobjects
where 
 t_diagramobjects.diagram_ID = (select diagram_ID from t_diagram where ea_Guid="#PARAM0#")
)
and 
t_connector.end_object_id in
(
select object_id
from 
 t_diagramobjects
where 
 t_diagramobjects.diagram_ID = (select diagram_ID from t_diagram where ea_Guid="#PARAM0#")
)