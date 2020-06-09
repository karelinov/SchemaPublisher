package com.msr.schemepublisher;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ArrayNode;
import com.fasterxml.jackson.databind.node.ObjectNode;

public class TemplateHelper {
	
	/**
	 * Возвращает содержимое файла json-шаблона из папки  pagetemplate
	 * @param templateName
	 * @return
	 */
	public static String getTemplate(String templateName) {
		StringBuilder result = new StringBuilder();

		InputStream inputStream;
		try {
			inputStream = CPConfig.class.getClassLoader().getResourceAsStream("pagetemplate/" + templateName+".json");
			InputStreamReader isReader = new InputStreamReader(inputStream);
			BufferedReader reader = new BufferedReader(isReader);
			String str;
			while ((str = reader.readLine()) != null) {
				result.append(str);
			}
		} catch (Exception e) {
			throw new RuntimeException(e);
		}

		return result.toString();
	}
	
	
	public static String setTitle(String pageTemplate, String title) {
		String result = null; 
		
	    try {
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(pageTemplate);
			ObjectNode rootNode = rootJNode.deepCopy();
			
			rootNode.put("title", title);
			
			result = rootNode.toString();
			
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
		
	    return result;
	}
	
	public static String setAncestor(String pageTemplate, Integer parentPageID) {
		String result = null; 
		
	    try {
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(pageTemplate);
			ObjectNode rootNode = rootJNode.deepCopy();
			
			
			JsonNode ancestorsJNode = rootNode.get("ancestors");
			ArrayNode ancestorsNode;
			if (ancestorsJNode != null) {
				ancestorsNode = ancestorsJNode.deepCopy();
			}
			else {
				ancestorsNode = mapper.createArrayNode();
			}
			ancestorsNode.removeAll();
			ObjectNode ancestorNode = mapper.createObjectNode();
			ancestorNode.put("id",parentPageID);
			ancestorsNode.insert(0, ancestorNode);
			
			rootNode.set("ancestors", ancestorsNode);
			
			result = rootNode.toString();
			
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
		
	    return result;
	}
	
	/**
	 * Читает из шаблона страницы элемент Body.Storage
	 * @param pageTemplate
	 * @return
	 */
	public static String getBody(String pageTemplate) {
		
	    try {
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(pageTemplate);
			
			JsonNode storagevalueJNode = rootJNode.get("body").get("storage").get("value");
			
			return storagevalueJNode.asText();
			
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
	}	
	
	
	/**
	 * Записывает в шаблон страницы элемент Body.Storage
	 * @param pageTemplate
	 * @param pageBodyValue
	 * @return
	 */
	public static String setBody(String pageTemplate, String pageBodyValue) {
		
	    try {
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(pageTemplate);
			ObjectNode rootNode = rootJNode.deepCopy();
			
			ObjectNode bodyNode = rootNode.get("body").deepCopy();
			
			ObjectNode storageNode = bodyNode.get("storage").deepCopy();
			storageNode.put("value", pageBodyValue);
			
			bodyNode.set("storage", storageNode);
			rootNode.set("body", bodyNode);
			
			return rootNode.toString();
			
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
	}	
	
	public static String addVersion(String pageTemplate) {
		String result = null; 
		
	    try {
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(pageTemplate);
			ObjectNode rootNode = rootJNode.deepCopy();
			
			ObjectNode versionNode = rootNode.get("version").deepCopy();
			versionNode.put("number", versionNode.get("number").asInt() +1);
			
			rootNode.set("version", versionNode);
			
			result = rootNode.toString();
			
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
		
	    return result;
	}

}
