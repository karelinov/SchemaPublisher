package com.msr.schemepublisher;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Хэлпер для манипуляции содержимым страницы
 * 
 * @author Helg
 *
 */
public class PageBodyHelper {
	// <ac:image[\s\S]*?alt=\"SPARXIMAGE-([A-Za-z0-9-]+)\"[\s\S]*?>[\s\S]*?<\/ac:image>
	public static final String AC_IMAGE_PATTERN = "<ac:image[\\s\\S]*?alt=\\\"SPARXIMAGE-([A-Za-z0-9-]+)\\\"[\\s\\S]*?>[\\s\\S]*?<\\/ac:image>";
	// <ac:image([\s\S]*?ac:alt=\"SPARXIMAGE-[A-Z0-9-]+\"[\s\S]*?)>[\s\S]*?<\/ac:image>
	public static final String AC_FULLIMAGE_PATTERN = "<ac:image([\\s\\S]*?ac:alt=\\\"SPARXIMAGE-[A-Z0-9-]+\\\"[\\s\\S]*?)>[\\s\\S]*?<\\/ac:image>";	
	// <ac:image[\s\S]*ac:width="([0-9]+)"[\s\S]*>[\s\S]*<\/ac:image>
	public static final String AC_IMAGE_WIDTH_PATTERN = "<ac:image[\\s\\S]*ac:width=\"([0-9]+)\"[\\s\\S]*>[\\s\\S]*<\\/ac:image>";
	public static final String AC_IMAGE_HEIGHT_PATTERN = "<ac:image[\\s\\S]*?(ac:height=\"[0-9]+\")[\\s\\S]*?>[\\s\\S]*?<\\/ac:image>";
	// <ac:image[\s\S]*(ac:thumbnail=\"\b(true|false)\b\")[\s\S]*>[\s\S]*<\/ac:image>
	public static final String AC_IMAGE_THUMBNAIL_PATTERN = "<ac:image[\\s\\S]*(ac:thumbnail=\\\"\\b(true|false)\\b\\\")[\\s\\S]*>[\\s\\S]*<\\/ac:image>";
	
	public static final String RI_ATTACHMENT_PATTERN = "<ri:attachment ri:filename=\"([A-Za-z0-9-\\.]+)\"\\/>";
	//<ac:structured-macro[\s\S]*?ac:name="panel"[\s\S]*?>[\s\S]*?<ac:parameter[\s\S]*?ac:name="title">([\s\S]*?)<\/ac:parameter>[\s\S]*?<ac:rich-text-body>([\s\S]*?)<\/ac:rich-text-body>[\s\S]*?<\/ac:structured-macro>
	public static final String AC_PANEL_PATTERN="<ac:structured-macro[\\s\\S]*?ac:name=\"panel\"[\\s\\S]*?>[\\s\\S]*?<ac:parameter[\\s\\S]*?ac:name=\"title\">([\\s\\S]*?)<\\/ac:parameter>[\\s\\S]*?<ac:rich-text-body>([\\s\\S]*?)<\\/ac:rich-text-body>[\\s\\S]*?<\\/ac:structured-macro>";

	private static ArrayList<StorageFragment> splitByPatterns(String inputStr, String pattern) {
		ArrayList<StorageFragment> result = new ArrayList<StorageFragment>();

		try {

			// Бредём по регекспу и конструируем список подстрок из того что сматчилось и
			// нет
			Pattern patt = Pattern.compile(pattern);
			Matcher matcher = patt.matcher(inputStr);
			int lastIndex = 0;
			while (matcher.find()) {
				result.add(new PageBodyHelper().new StorageFragment(inputStr.substring(lastIndex, matcher.start()), FragmentType.ORDINARY, null));

				String[] capturedGropus = new String[matcher.groupCount()+1];
				for (int i = 0; i <= matcher.groupCount(); i++) {
					capturedGropus[i] = matcher.group(i);
				}
				result.add(new PageBodyHelper().new StorageFragment(matcher.group(0), FragmentType.MATCH, capturedGropus));

				lastIndex = matcher.end();
			}
			if (lastIndex < inputStr.length()) {
				result.add(new PageBodyHelper().new StorageFragment(inputStr.substring(lastIndex, inputStr.length()), FragmentType.ORDINARY, null));
			}

		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}

		return result;
	}

	/**
	 * Устанавливает в разметке страницы картинку с указанным id и файлом (старый
	 * элемент для картинок с таким ID перетиравется )
	 * 
	 * @param pageBodyValue
	 * @param imageID
	 * @param imageName
	 * @param pictureWidth
	 * @return
	 */
	public static String setImage(String pageBodyValue, String imageID, String imageName, Integer pictureWidth) {
		StringBuilder result = new StringBuilder();
		Boolean imageIDFound = false;
		
		// Рассчитываем ширину показа картинки
		Integer width = pictureWidth;
		if (width > 2000) {
			width = 1000;
		}
		else if (width > 800) {
			width = 800;
		}
		

		// разбиваем разметку на фрагменты
		ArrayList<StorageFragment> fragments = splitByPatterns(pageBodyValue, AC_IMAGE_PATTERN);

		if (fragments.size() > 1) { // если в списке больше одного элемента, значит элемент с картинками есть
			// бежим по фрагментам
			for (int i = 0; i < fragments.size(); i++) {
				StorageFragment fragment = fragments.get(i);
				// если обычный фрагмент - просто добавляем к результату
				if (fragment.fragmentType == FragmentType.ORDINARY) {

					result.append(fragment.fragment);
					// если то, что сошлось с шаблоном картинки -
				} else {
					// Проверяем ID картинки
					if (fragment.foundGroups[1].equals(imageID)) {
						// если сошлось -  
						// Заменяем имя аттачмента
						imageIDFound = true;
						String replacedFragment = replacePattern(fragment.fragment, RI_ATTACHMENT_PATTERN, imageName);
						// устанавливаем ширину показа
						if(!replacedFragment.matches(AC_IMAGE_WIDTH_PATTERN)) { // если элемента с шириной нет, вставляем со значение по умолчанию
							Pattern patt = Pattern.compile(AC_FULLIMAGE_PATTERN);
							Matcher matcher = patt.matcher(replacedFragment);
							matcher.find();
							replacedFragment = replaceGroup(fragment.fragment, AC_FULLIMAGE_PATTERN, 1, matcher.group(1)+" ac:width=\"800\"");
						}
						replacedFragment = replaceGroup(replacedFragment, AC_IMAGE_WIDTH_PATTERN, 1, width.toString());
						
						// Отстреливает возможные элементы height и thumbnail
						replacedFragment = replaceGroup(replacedFragment, AC_IMAGE_HEIGHT_PATTERN, 1, "");
						replacedFragment = replaceGroup(replacedFragment, AC_IMAGE_THUMBNAIL_PATTERN, 1, "");
						
						result.append(replacedFragment);
					} else {
						result.append(fragment.fragment);
					}
				}
			}
		} 
		if(!imageIDFound) {
			// Если картинки с таким идентифкатором в разметке нет - вставляем её после существующего контента
			result.append(pageBodyValue);
			result.append(getImageMarkup(imageID, imageName));
		}
		
		return result.toString();
	}
	
	
	/**
	 * Устанавливает в разметке страницы картинку с указанным id и файлом (старый
	 * элемент для картинок с таким ID перетиравется )
	 * 
	 * @param pageBodyValue
	 * @param imageID
	 * @param imageName
	 * @return
	 */
	public static String setText(String pageBodyValue, String textID, String text) {
		StringBuilder result = new StringBuilder();
		Boolean textIDFound = false;

		// разбиваем разметку на фрагменты
		ArrayList<StorageFragment> fragments = splitByPatterns(pageBodyValue, AC_PANEL_PATTERN);

		if (fragments.size() > 1) { // если в списке больше одного элемента, значит какие то текстовые элементы есть
			// бежим по фрагментам
			for (int i = 0; i < fragments.size(); i++) {
				StorageFragment fragment = fragments.get(i);
				// если обычный фрагмент - просто добавляем к результату
				if (fragment.fragmentType == FragmentType.ORDINARY) {

					result.append(fragment.fragment);
					// если то, что сошлось с шаблоном картинки -
				} else {
					// Проверяем ID текста
					if (fragment.foundGroups[1].equals(textID)) {
						// если сошлось - заменяем текст
						textIDFound = true;
						String replacedFragment = replaceGroup(fragment.fragment, AC_PANEL_PATTERN, 2, text);
						result.append(replacedFragment);
					} else {
						result.append(fragment.fragment);
					}
				}
			}
		} 
		if(!textIDFound) {
			// Если картинки с таким идентифкатором в разметке нет - вставляем её после существующего контента
			result.append(pageBodyValue);
			result.append(getTextMarkup(textID, text));
		}
		
		return result.toString();
	}
	
	
	/**
	 * Конструирует разметку для картинки с указанными ID и файлом
	 * @param imageID
	 * @param imageName
	 * @return
	 */
	private static String getImageMarkup(String imageID, String imageName) {
		String result = getTemplate("image");
		
		
		result = replaceGroup(result,AC_IMAGE_PATTERN,1,imageID);
		result = replaceGroup(result,RI_ATTACHMENT_PATTERN,1,imageName);
		
		return result;
	}
	
	/**
	 * Конструирует разметку для панели текста с указанными ID и текстом
	 * @param imageID
	 * @param imageName
	 * @return
	 */
	private static String getTextMarkup(String textID, String text) {
		String result = getTemplate("textpanel");
		
		
		result = replaceGroup(result,AC_PANEL_PATTERN,1,textID);
		result = replaceGroup(result,AC_PANEL_PATTERN,2,text);
		
		return result;
	}	
	
	

	/**
	 * Функция в теле строки ищет указанный шаблон и заменяет его на указанную
	 * строку
	 * 
	 * @param inputStr
	 * @param pattern
	 * @param replaceStr
	 * @return Изменённая строка
	 */
	private static String replacePattern(String inputStr, String pattern, String replaceStr) {
		StringBuilder result = new StringBuilder();

		try {

			// Бредём по регекспу и конструируем на основе позиций матчей новую строку
			Pattern patt = Pattern.compile(pattern);
			Matcher matcher = patt.matcher(inputStr);
			int lastIndex = 0;
			while (matcher.find()) {
				result.append(inputStr, lastIndex, matcher.start());
				result.append(replaceStr);

				lastIndex = matcher.end();
			}
			if (lastIndex < inputStr.length()) {
				result.append(inputStr, lastIndex, inputStr.length());
			}

		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}

		return result.toString();
	}

	/**
	 * Функция в теле строки ищет указанный шаблон и заменяет в нём первую группу на
	 * указанную строку
	 * 
	 * @param inputStr
	 * @param pattern
	 * @param replaceStr
	 * @return Изменённая строка
	 */
	private static String replaceGroup(String inputStr, String pattern, Integer groupNumber, String replaceStr) {
		StringBuilder result = new StringBuilder();

		try {

			// Бредём по регекспу и конструируем на основе позиций матчей новую строку
			Pattern patt = Pattern.compile(pattern);
			Matcher matcher = patt.matcher(inputStr);
			int lastIndex = 0;
			while (matcher.find()) {
				result.append(inputStr, lastIndex, matcher.start());
				if (matcher.groupCount() >= groupNumber) {
					String capturedPattern = matcher.group(0);
					//String replacedPattern = inputStr.substring(0, matcher.start(1)) + replaceStr + inputStr.substring(matcher.end(1), inputStr.length());
					String replacedPattern = capturedPattern.substring(0, matcher.start(groupNumber)-matcher.start(0)) + replaceStr + capturedPattern.substring(matcher.end(groupNumber)-matcher.start(0), capturedPattern.length());

					result.append(replacedPattern);

				} else {
					result.append(replaceStr);
				}

				lastIndex = matcher.end();
			}
			if (lastIndex < inputStr.length()) {
				result.append(inputStr, lastIndex, inputStr.length());
			}

		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}

		return result.toString();
	}
	
	/**
	 * Возвращает содержимое файла шаблона из папки  bodytemplate
	 * @param templateName
	 * @return
	 */
	public static String getTemplate(String templateName) {
		StringBuilder result = new StringBuilder();

		InputStream inputStream;
		try {
			inputStream = CPConfig.class.getClassLoader().getResourceAsStream("bodytemplate/" + templateName+".xhtml");
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
	
	

	/**
	 * Класс представляет собой строку формата Confluencе, разбитую на части по
	 * найденным наблонам подстрок
	 * 
	 * @author Helg
	 *
	 */
	public class StorageFragment {
		/**
		 * Часть (исходной) строки
		 */
		public String fragment;
		/**
		 * Тип части
		 */
		public FragmentType fragmentType;
		/**
		 * Найденные в шаблоне группы
		 */
		public String[] foundGroups;

		public StorageFragment(String fragment, FragmentType fragmentType, String[] foundGroups) {
			super();
			this.fragment = fragment;
			this.fragmentType = fragmentType;
			this.foundGroups = foundGroups;
		}
	}

	public enum FragmentType {
		/**
		 * Часть строки, не совпавшая с шаблоном
		 */
		ORDINARY,
		/**
		 * Часть строки, совпавшая с шаблоном
		 */
		MATCH;
	}

}
