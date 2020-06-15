package com.msr.schemepublisher;

import java.io.File;
import java.io.FileInputStream;
import java.net.HttpURLConnection;
import java.net.URI;
import java.nio.file.Paths;
import java.util.Iterator;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;

public class RestAPI {
	private static final Logger logger = LoggerFactory.getLogger(RestAPI.class);

	/**
	 * Получение контента по названию НЕ ДОДЕЛАНО
	 * 
	 * @param title
	 * @return
	 */
	public static String getContent(String title) {
		String result = null;

		try {

			URI uri = HTTPHelper.getBaseURI("content");
			uri = HTTPHelper.addURIParameter(uri, "title", "TEST");

			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "GET");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);

			connectionResult.value.connect();

			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			if (responseResult.code != 200)
				throw new Exception(responseResult.message);

			result = responseResult.value;

		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}

		return result;
	}

	/**
	 * 
	 * @param content
	 * @return
	 */
	public static ExecResult<String> postContent(String content) {
		ExecResult<String> result = new ExecResult<String>();
		result.code = -1;

		try {

			URI uri = HTTPHelper.getBaseURI("content");

			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "POST");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);

			ExecResult<Boolean> setBodyresult = HTTPHelper.setConnectionBody(connectionResult.value, content);
			if (setBodyresult.code != 0)
				throw new Exception(setBodyresult.message);

			connectionResult.value.connect();

			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			if (responseResult.code != 200)
				throw new Exception(responseResult.message + responseResult.value);

			// разбираем json ответ, извлекаем id созданой страницы
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(responseResult.value);

			result.code = rootJNode.get("id").asInt();
			result.value = responseResult.value;

		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}

		return result;
	}

	/**
	 * 
	 * @param content
	 * @return
	 */
	public static ExecResult<String> putContent(Integer pageID, String content) {
		ExecResult<String> result = new ExecResult<String>();
		result.code = -1;

		try {

			URI uri = HTTPHelper.getBaseURI("content");
			uri = HTTPHelper.addURIPath(uri, "/" + pageID.toString());

			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "PUT");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);

			ExecResult<Boolean> setBodyresult = HTTPHelper.setConnectionBody(connectionResult.value, content);
			if (setBodyresult.code != 0)
				throw new Exception(setBodyresult.message);

			connectionResult.value.connect();

			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			if (responseResult.code != 200)
				throw new Exception(responseResult.message + responseResult.value);

			// разбираем json ответ, извлекаем id созданой страницы
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(responseResult.value);

			result.code = rootJNode.get("id").asInt();
			result.value = responseResult.value;

		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}

		return result;
	}

	/**
	 * Функция ищет страницы по имени. Если нашла - возвращает ID страницы
	 * 
	 * @param title
	 * @return
	 */
	public static ExecResult<Integer> getPageIdByTitle(String title) {
		ExecResult<Integer> result = new ExecResult<Integer>();
		result.code = -1;

		try {

			URI uri = HTTPHelper.getBaseURI("content");

			uri = HTTPHelper.addURIParameter(uri, "title", title);
			uri = HTTPHelper.addURIParameter(uri, "spaceKey", CPConfig.app().getProperty("SPACE_KEY"));

			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "GET");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);

			connectionResult.value.connect();

			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			//logger.info("responseResult.code =" + responseResult.code);			
			if (responseResult.code != 200)
				throw new Exception(responseResult.message + responseResult.value);
			//logger.info("responseResult.value="+responseResult.value);
			

			// разбираем json ответ
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(responseResult.value);

			JsonNode resultsNode = rootJNode.get("results");

			for (int i = 0; i < resultsNode.size(); i++) {
				JsonNode pageNode = resultsNode.get(i);

				if (pageNode.get("title").asText().equals(title)) {
					result.code = 0;
					result.value = pageNode.get("id").asInt();
					break;
				}

			}

		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}
		
		return result;
	}

	/**
	 * Функция ищет страницы по имени. Если нашла - возвращает контент страницы
	 * 
	 * @param title
	 * @return
	 */
	public static ExecResult<String> getPageByTitle(String title) {
		ExecResult<String> result = new ExecResult<String>();
		result.code = -1;

		try {

			URI uri = HTTPHelper.getBaseURI("content");

			uri = HTTPHelper.addURIParameter(uri, "title", title);
			uri = HTTPHelper.addURIParameter(uri, "spaceKey", CPConfig.app().getProperty("SPACE_KEY"));
			uri = HTTPHelper.addURIParameter(uri, "expand", "version,body,storage");

			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "GET");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);

			connectionResult.value.connect();

			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			if (responseResult.code != 200)
				throw new Exception(responseResult.message + responseResult.value);

			// разбираем json ответ
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(responseResult.value);

			JsonNode resultsNode = rootJNode.get("results");

			for (int i = 0; i < resultsNode.size(); i++) {
				JsonNode pageNode = resultsNode.get(i);
				if (pageNode.get("title").asText().equals(title)) {
					result.code = pageNode.get("id").asInt();
					result.value = pageNode.toString();
					break;
				}

			}

		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}

		return result;
	}

	/**
	 * Функция возвращает страницу по pageID
	 * 
	 * @param pageID
	 * @return
	 */
	public static ExecResult<String> getPageByID(Integer pageID) {
		ExecResult<String> result = new ExecResult<String>();
		result.code = -1;

		try {

			URI uri = HTTPHelper.getBaseURI("content");
			uri = HTTPHelper.addURIPath(uri, "/"+pageID.toString());

			 uri = HTTPHelper.addURIParameter(uri, "expand", "version,body,body.storage");

			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "GET");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);

			connectionResult.value.connect();

			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			if (responseResult.code != 200)
				throw new Exception(responseResult.message + responseResult.value);

			result.code = pageID;
			result.value = responseResult.value;

		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}

		return result;
	}

	/**
	 * заливает аттачмент на страницу
	 * 
	 * @param pageID
	 * @param attachmentFilePath
	 * @return
	 */
	public static ExecResult<String> postAttachment(Integer pageID, String attachmentFilePath) {
		ExecResult<String> result = new ExecResult<String>();
		result.code = -1;

		try {
			String attachmentFileName = Paths.get(attachmentFilePath).getFileName().toString();

			// Создаём URI
			URI uri = HTTPHelper.getBaseURI("content");
			uri = HTTPHelper.addURIPath(uri, "/" + pageID.toString() + "/child/attachment");

			uri = HTTPHelper.addURIParameter(uri, "allowDuplicated", "true");

			// Создаём Connection
			ExecResult<HttpURLConnection> connectionResult = HTTPHelper.getConnection(uri, "POST");
			if (connectionResult.code != 0)
				throw new Exception(connectionResult.message);
			HttpURLConnection connection = connectionResult.value;

			// Открываем файл
			File file = new File(attachmentFilePath);
			FileInputStream fileInputStream = new FileInputStream(file);

			// Льём файл в connection
			ExecResult<Boolean> multipartFormBodyResult = HTTPHelper.setMultipartFormBody(connection, fileInputStream, attachmentFileName);
			if (multipartFormBodyResult.code != 0)
				throw new Exception(multipartFormBodyResult.message);

			// забираем ответ
			ExecResult<String> responseResult = HTTPHelper.getConnectionResponse(connectionResult.value);
			if (responseResult.code != 200)
				throw new Exception(responseResult.message + responseResult.value);

			// разбираем json ответ, извлекаем id созданого аттачмента
			ObjectMapper mapper = new ObjectMapper();
			JsonNode rootJNode = mapper.readTree(responseResult.value);

			JsonNode resultsNode = rootJNode.get("results");
			JsonNode attachmentNode = resultsNode.get(0);

			result.code = attachmentNode.get("id").asInt();
			result.value = responseResult.value;

		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}

		return result;
	}

}
