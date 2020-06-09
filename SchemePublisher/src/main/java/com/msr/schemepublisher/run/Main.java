package com.msr.schemepublisher.run;

import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileInputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.HttpURLConnection;
import java.net.URI;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Properties;
import java.util.UUID;

import javax.imageio.ImageIO;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.msr.schemepublisher.CPConfig;
import com.msr.schemepublisher.ExecResult;
import com.msr.schemepublisher.HTTPHelper;
import com.msr.schemepublisher.PageBodyHelper;
import com.msr.schemepublisher.RestAPI;
import com.msr.schemepublisher.TemplateHelper;

public class Main {
	private static final Logger logger = LoggerFactory.getLogger(Main.class);

	public static void main(String[] args) {
		logger.info("main started");

		String propFileName;
		if (args == null || args.length == 0) {
			propFileName = "F:\\DEV\\ERL\\SchemePublisher\\SchemePublisher\\src\\test\\resources\\in\\test.properties";
		} else {
			if (!(new File(args[0]).exists())) {
				throw new RuntimeException("Файл не найден");
			} else {
				propFileName = args[0];
			}
		}

		ExecResult<Boolean> result = PublishContent(propFileName);
		if(result.code != 0)
			throw new RuntimeException(result.message);

		/*
		 * String newContent = TemplateHelper.getTemplate("apicreateexample");
		 * newContent =TemplateHelper.setTitle(newContent, "test "+
		 * UUID.randomUUID().toString()); ExecResult<Integer> parentIDResult =
		 * RestAPI.getPageIdByTitle("TEST"); if (parentIDResult.code !=0 ) throw new
		 * RuntimeException(parentIDResult.message);
		 * logger.info("getPageIdByTitle returned pageID="+parentIDResult.value);
		 * 
		 * newContent =TemplateHelper.setAncestor(newContent, parentIDResult.value);
		 * 
		 * ExecResult<String> postContentResult = RestAPI.postContent(newContent); if
		 * (postContentResult.code < 0 ) throw new
		 * RuntimeException(postContentResult.message);
		 * logger.info("postContent returned pageID="+postContentResult.code);
		 * newContent = postContentResult.value;
		 * 
		 * String attachmentFilePath = CPConfig.app().getProperty("PICTURE");
		 * ExecResult<String> postAttachmentResult =
		 * RestAPI.postAttachment(postContentResult.code, attachmentFilePath); if
		 * (postAttachmentResult.code < 0 ) throw new
		 * RuntimeException(postAttachmentResult.message);
		 * logger.info("postAttachment returned objectID="+postAttachmentResult.code);
		 * 
		 * String attachmentFileName =
		 * Paths.get(attachmentFilePath).getFileName().toString(); String
		 * attachmentWOExtension = attachmentFileName.replaceFirst("\\..+$",""); String
		 * pageBody = TemplateHelper.getBody(newContent); pageBody =
		 * PageBodyHelper.setImage(pageBody,attachmentWOExtension, attachmentFileName);
		 * pageBody =
		 * PageBodyHelper.setText(pageBody,attachmentWOExtension,"Тестовы текст");
		 * 
		 * newContent = TemplateHelper.setBody(newContent,pageBody); newContent =
		 * TemplateHelper.addVersion(newContent);
		 * 
		 * ExecResult<String> putContentResult =
		 * RestAPI.putContent(postContentResult.code, newContent); if
		 * (putContentResult.code < 0 ) throw new
		 * RuntimeException(putContentResult.message);
		 * logger.info("putContentResult returned objectID="+putContentResult.code);
		 */

	}

	/**
	 * Функция публикует контент, описанный в указанном properties - файле
	 * 
	 * @param propertiesFileName
	 * @return
	 */
	public static ExecResult<Boolean> PublishContent(String propertiesFileName) {

		ExecResult<Boolean> result = new ExecResult<Boolean>();

		Properties prop = new Properties();

		try {

			// Открываем файл
			InputStream inputStream;
			inputStream = new FileInputStream(new File(propertiesFileName));
			Reader reader = new InputStreamReader(inputStream, "UTF-8");
			prop.load(reader);

			// Читаем параметры
			String objID = prop.getProperty("OBJID");
			String parentPageName = prop.getProperty("PARENTPAGE");
			String pageName = prop.getProperty("PAGEGENAME");
			String imageFileName = prop.getProperty("IMAGE");
			String textFileName = prop.getProperty("TEXT");

			// Получаем данные родительской страницы
			ExecResult<Integer> parentIDResult = RestAPI.getPageIdByTitle(parentPageName);
			if (parentIDResult.code != 0)
				throw new RuntimeException(parentIDResult.message);
			logger.info("getPageIdByTitle returned parent pageID=" + parentIDResult.value);

			String pageContent;
			Integer pageID;
			// Получаем id страницы, которую надо обновить
			ExecResult<Integer> pageIDResult = RestAPI.getPageIdByTitle(pageName);
			logger.info("getPageIdByTitle returned page pageID=" + pageIDResult.value);
			
			if (pageIDResult.code < 0) { // если страницу не нашли - создаём (пустую)
				logger.info("page for update not found, creating new one....");

				pageContent = TemplateHelper.getTemplate("apicreateexample");
				pageContent = TemplateHelper.setTitle(pageContent, pageName);
				pageContent = TemplateHelper.setAncestor(pageContent, parentIDResult.value);

				ExecResult<String> postContentResult = RestAPI.postContent(pageContent);
				if (postContentResult.code < 0)
					throw new RuntimeException(postContentResult.message);
				logger.info("postContent returned pageID=" + postContentResult.code);
				pageContent = postContentResult.value;
				pageID = postContentResult.code;

			} else {
				pageID = pageIDResult.value;
				// Получаем страницу которую надо обновить
				ExecResult<String> pageResult = RestAPI.getPageByID(pageID);
				if (pageResult.code < 0)
					throw new RuntimeException(pageResult.message);
				
				pageContent = pageResult.value;
				pageID = pageResult.code;
				logger.info("getPageByID returned page pageID=" + pageResult.code);
			}

			// Заливаем новый аттачмент с картинкой
			String propertiesFilePath = Paths.get(propertiesFileName).getParent().toString();
			String attachmentFilePath = Paths.get(propertiesFilePath, imageFileName).toString();
			
			ExecResult<String> postAttachmentResult = RestAPI.postAttachment(pageID, attachmentFilePath);
			if (postAttachmentResult.code < 0)
				throw new RuntimeException(postAttachmentResult.message);
			logger.info("postAttachment returned objectID=" + postAttachmentResult.code);

			// Теперь у нас есть контент страницы - получаем разметку
			String pageBody = TemplateHelper.getBody(pageContent);
			// заливаем в разметку данные картинки
			BufferedImage bimg = ImageIO.read(new File(attachmentFilePath));
			int pictureWidth = bimg.getWidth();
			
			pageBody = PageBodyHelper.setImage(pageBody, objID, imageFileName, pictureWidth);
			// заливаем в разметку текст
			String pageText = new String(Files.readAllBytes(Paths.get(Paths.get(propertiesFilePath, textFileName).toString())));
			pageBody = PageBodyHelper.setText(pageBody, objID, pageText);

			// Обновляем разметку в контенте
			pageContent = TemplateHelper.setBody(pageContent, pageBody);
			pageContent = TemplateHelper.addVersion(pageContent);

			// Update
			ExecResult<String> putContentResult = RestAPI.putContent(pageID, pageContent);
			if (putContentResult.code < 0)
				throw new RuntimeException(putContentResult.message);
			logger.info("putContentResult returned objectID=" + putContentResult.code);

			result.value = true;

		} catch (Exception ex) {
			result.code = -1;
			result.setException(ex);
		}

		return result;

	}

}
