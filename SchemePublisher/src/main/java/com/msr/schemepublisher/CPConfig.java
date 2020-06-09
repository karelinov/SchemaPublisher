package com.msr.schemepublisher;

import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.util.Properties;

public class CPConfig {
	private static Properties _app = null;

	public static Properties app() {
		if (_app == null) {
			_app = new java.util.Properties();

			InputStream inputStream;
			try {
				inputStream = CPConfig.class.getClassLoader().getResourceAsStream("config/app.properties");
				Reader reader = new InputStreamReader(inputStream, "UTF-8");
				_app.load(reader);
			} catch (Exception e) {
				throw new RuntimeException(e);
			}
		}

		return _app;
	}

}
