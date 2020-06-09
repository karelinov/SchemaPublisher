package com.msr.schemepublisher;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.InetSocketAddress;
import java.net.Proxy;
import java.net.SocketAddress;
import java.net.URI;
import java.net.URL;
import java.util.Base64;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class HTTPHelper {
	private static final Logger logger = LoggerFactory.getLogger(HTTPHelper.class);

	/**
	 * Функция получения ответа из HttpURLConnection
	 */
	public static ExecResult<String> getConnectionResponse(HttpURLConnection connection) {
		ExecResult<String> result = new ExecResult<String>();
		InputStream inputStream;
		int statusCode;
	
		try {
	
			statusCode = connection.getResponseCode();
			if (statusCode != 200 /* or statusCode >= 200 && statusCode < 300 */) {
				inputStream = connection.getErrorStream();
			} else
				inputStream = connection.getInputStream();
	
			BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
			StringBuilder stringBuilder = new StringBuilder();
	
			String line = null;
			while ((line = reader.readLine()) != null) {
				stringBuilder.append(line + "\n");
			}
			reader.close();
	
			result.code = statusCode;
			result.value = stringBuilder.toString();
		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}
	
		return result;
	}

	/**
	 * Функция создания объекта HttpURLConnection
	 * @param uri
	 * @param requestMethod
	 * @return
	 */
	public static ExecResult<HttpURLConnection> getConnection(URI uri, String requestMethod) {
		ExecResult<HttpURLConnection> result = new ExecResult<HttpURLConnection>();
		URL url;
		Proxy proxy = null;
		
		try {
			
			if (Boolean.parseBoolean(CPConfig.app().getProperty("USE_PROXY"))) {
				SocketAddress addr = new InetSocketAddress(CPConfig.app().getProperty("PROXY_HOST"),
						Integer.parseInt(CPConfig.app().getProperty("PROXY_PORT")));
				proxy = new Proxy(Proxy.Type.HTTP, addr);
			}
			
			url = uri.toURL();
			// logger.info("constructed url = " + url);
			String userCredentials = "okarelin:12345";
			String basicAuth = "Basic " + new String(Base64.getEncoder().encode(userCredentials.getBytes()));
	
			HttpURLConnection connection;
			if (proxy != null) {
				connection = (HttpURLConnection) url.openConnection(proxy);
			} else {
				connection = (HttpURLConnection) url.openConnection();
			}
			connection.setRequestProperty("Authorization", basicAuth);
			connection.setRequestMethod(requestMethod);
	
			result.value = connection;
		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}
	
		return result;		
	}

	/**
	 * Функция конструирования базового URI для доступа к confluence
	 * @param path
	 * @return
	 */
	public static URI getBaseURI(String path) {
		// String uripath = CPConfig.app().getProperty("REST_API_PREFIX")+"/"+path
		URI result;
	
		try {
			result = new URI(CPConfig.app().getProperty("URI_SCHEME"), null, CPConfig.app().getProperty("CONF_HOST"),
					Integer.parseInt(CPConfig.app().getProperty("CONF_PORT")),
					CPConfig.app().getProperty("REST_API_PREFIX") + path, null, null);
		} catch (Exception ex) { // все методы работы с URI раизят какие то ошибки
			throw new RuntimeException(ex);
		}
	
		return result;
	}

	/**
	 * Функция добавления GET-параметра в URI
	 * @param baseUri
	 * @param parName
	 * @param parValue
	 * @return
	 */
	public static URI addURIParameter(URI baseUri, String parName, String parValue) {
		try {
	
			String query = baseUri.getQuery();
			if (query == null)
				query = "";
			if (!query.isEmpty()) {
				query = query + "&";
			}
			query = query + parName + "=" + parValue;
	
			return new URI(baseUri.getScheme(), baseUri.getAuthority(), baseUri.getPath(), query.toString(), null);
	
		} catch (Exception ex) { // все методы работы с URI раизят какие то ошибки
			throw new RuntimeException(ex);
		}
	}

	/**
	 * Функция записи body в request
	 */
	public static ExecResult<Boolean> setConnectionBody(HttpURLConnection connection, String body) {
		ExecResult<Boolean> result = new ExecResult<Boolean>();
	
		try {
	
			connection.setDoOutput(true);
			connection.setRequestProperty("Content-Type", "application/json");
			connection.setRequestProperty("Accept", "application/json, text/javascript, */*; q=0.01");
	
			// пишем тело
			OutputStream os = connection.getOutputStream();
			byte[] input = body.getBytes("utf-8");
			os.write(input, 0, input.length);
	
			result.value = true;
		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}
	
		return result;
	}
	
	/**
	 * Функция расширения URI
	 */
	public static URI addURIPath(URI baseUri, String addPath) {
		try {
			
			String path = baseUri.getPath();
			path = path + addPath;
	
			return new URI(baseUri.getScheme(), baseUri.getAuthority(), path, baseUri.getQuery(), null);
	
		} catch (Exception ex) { // все методы работы с URI раизят какие то ошибки
			throw new RuntimeException(ex);
		}
	}

	/**
	 * Формирование тела сообщения как Multipart-form-data
	 * @param connection
	 * @param inputStream - тут считаем файл
	 * @param fileName - тут имя файла
	 * @return
	 */
	public static ExecResult<Boolean> setMultipartFormBody(HttpURLConnection connection, InputStream inputStream, String fileName) {
		ExecResult<Boolean> result = new ExecResult<Boolean>();
		String boundary =  "*****"+Long.toString(System.currentTimeMillis())+"*****";
		DataOutputStream outputStream = null;
		int bytesRead, bytesAvailable, bufferSize;
		byte[] buffer;
		int maxBufferSize = 1*1024*1024;
	
		try {
			
			
			connection.setDoInput(true);
			connection.setDoOutput(true);
			connection.setUseCaches(false);

			connection.setRequestProperty("Accept", "application/json, text/javascript, */*; q=0.01");
			connection.setRequestProperty("Content-Type", "multipart/form-data; boundary="+boundary);
			connection.setRequestProperty("X-Atlassian-Token","no-check");
			
			// Создаём Stream для Body
			// Пишем всякие заголовки для multipart/form-data
			outputStream = new DataOutputStream(connection.getOutputStream());
			outputStream.writeBytes("--" + boundary + "\r\n");
			//outputStream.writeChars("Content-Disposition: form-data; name=\"file\"; filename=\"" + fileName +"\"" + "\r\n");
			outputStream.writeBytes("Content-Disposition: form-data; name=\"file\"; filename=\"" + fileName +"\"" + "\r\n");
			outputStream.writeBytes("Content-Type: image/jpeg" + "\r\n");
			outputStream.writeBytes("Content-Transfer-Encoding: binary" + "\r\n");
			outputStream.writeBytes("\r\n");
			
			bytesAvailable = inputStream.available();
			bufferSize = Math.min(bytesAvailable, maxBufferSize);
			buffer = new byte[bufferSize];
			
			bytesRead = inputStream.read(buffer, 0, bufferSize);
			while(bytesRead > 0) {
				outputStream.write(buffer, 0, bufferSize);
				bytesAvailable = inputStream.available();
				bufferSize = Math.min(bytesAvailable, maxBufferSize);
				bytesRead = inputStream.read(buffer, 0, bufferSize);
			}
			outputStream.writeBytes("\r\n");
			inputStream.close();
			
			
			/*
			// Льём в outputStream следующие части если нужны 
			String[] posts = post.split("&");
			int max = posts.length;
			for(int i=0; i<max;i++) {
				outputStream.writeBytes(twoHyphens + boundary + lineEnd);
				String[] kv = posts[i].split("=");
				outputStream.writeBytes("Content-Disposition: form-data; name=\"" + kv[0] + "\"" + lineEnd);
				outputStream.writeBytes("Content-Type: text/plain"+lineEnd);
				outputStream.writeBytes(lineEnd);
				outputStream.writeBytes(kv[1]);
				outputStream.writeBytes(lineEnd);
			}
			*/
			outputStream.writeBytes("--" + boundary + "--" + "\r\n");			
			outputStream.flush();
			outputStream.close();			
			
			
			result.value = true;
		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}
	
		return result;
	}
	

}
