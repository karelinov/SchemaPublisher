package com.msr.schemepublisher;

/**
 * Простой Generic POJO - класс для возврата результата запуска + кода ошибки и сообщения
 * @author Helg
 *
 * @param <T>
 */
public class ExecResult<T> {
	/**
	 * Код результата запуска
	 */
	public int code = 0;
	
	/**
	 * Значение, возвращаемое при запуске  
	 */
	public T value;
	
	/**
	 * Сообщение, описывающее результат выполнения
	 * Опиционально
	 */
	public String message;
	
	
	public void setException(Exception ex) {
		this.message = ex.toString();
	}

}
