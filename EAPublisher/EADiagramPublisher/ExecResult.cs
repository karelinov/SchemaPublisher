using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    public class ExecResult<T>
    {
        public long code = 0;
        public T value;
        public string message;

        public void setException(Exception ex)
        {
            this.code = -1;
            this.message = ex.Message + ex.StackTrace;
        }
    }
}
