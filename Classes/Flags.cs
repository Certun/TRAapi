using System.Collections.Generic;
using System.Linq;

namespace WebPortal.Classes
{
    public class Flags
    {
        private readonly EntitiesModel _conn;
        private List<DAT0000> _flags;
        private List<DAT0000> _pflags;
        private List<DAT0000> _tflags;
        private List<DAT0000> _cflags;

        public Flags()
        {
            _conn = new EntitiesModel();
            _flags = _conn.DAT0000.ToList();
        }
        
        /// <summary>
        /// reload all flags from database
        /// </summary>
        public void Reload()
        {
            _flags = _conn.DAT0000.ToList();
        }

        /// <summary>
        /// filters by sys_program
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public Flags Program(string program)
        {
            _pflags = _flags.Where(f => f.sysprogram == program).ToList();
            return this;
        }

        /// <summary>
        /// filters by sys_type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Flags Type(string type)
        {
            _tflags = _pflags.Where(f => f.systype == type).ToList();
            return this;
        }

        /// <summary>
        /// filters by sys_code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Flags Code(byte code)
        {
            _cflags = _tflags.Where(f => f.syscode == code).ToList();
            return this;
        }

        /// <summary>
        /// gets sys_flag
        /// </summary>
        /// <returns></returns>
        public char? GetFlag()
        {
            var foo = _cflags.ElementAtOrDefault(0);
            return foo != null ? foo.sysflag : '0';
        }

        /// <summary>
        /// gets sys_value
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {            
            var foo = _cflags.ElementAtOrDefault(0);
            return foo != null ? foo.sysvalue : "";
        }
    }
}
