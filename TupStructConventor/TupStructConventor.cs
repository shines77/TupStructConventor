using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupHelper
{
    struct StructField
    {
        public string name;
        public string value;

        public StructField(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public class TupStructConventor
    {
        private string jsCode = "";
        private string csharpCode = "";
        private string infos = "";

        public TupStructConventor()
        {
        }

        public string JsCode()
        {
            return jsCode;
        }

        public void SetJsCode(string jsCode)
        {
            this.jsCode = jsCode;
        }

        public string CSharpCode()
        {
            return csharpCode;
        }

        public string Diagnostics()
        {
            return infos;
        }

        public string ConventToCSharp()
        {
            csharpCode = TupStructConventor.ConventToCSharp(jsCode, ref infos);
            return csharpCode;
        }

        public static string GenerateWarning(string info, int position)
        {
            return string.Format("Warning: Position: {0}, {1};\r\n", position, info);
        }

        public static string GenerateWarning(string info, char ident, int position)
        {
            return string.Format("Warning: Position: {0}, '{1}' {2};\r\n", position, ident, info);
        }

        public static string GenerateWarning(string info, string ident, int position)
        {
            return string.Format("Warning: Position: {0}, '{1}' {2};\r\n", position, ident, info);
        }

        public static string GenerateError(string info, int position)
        {
            return string.Format("Error: Position: {0}, {1};\r\n", position, info);
        }

        public static string ConventToCSharp(string jsCode, ref string infos)
        {
            string csharpCode = "";
            string info = "";
            const string HUYA_Prefix = "HUYA";

            StringStream stream = new StringStream(jsCode);

            do {
                string funcName;
                string fieldName;
                string fieldValue;
                string identName;

                List<StructField> fieldList = new List<StructField>();

                stream.skipWhiteSpaces();

                funcName = stream.parseIdentifier();
                stream.skipWhiteSpaces();

                if (funcName == HUYA_Prefix) {
                    info += "It's a HUYA Live class.\r\n";
                }

                while (stream.get() == '.') {
                    // Skip the '.' char inside struct name .
                    stream.skip();
                    funcName = stream.parseIdentifier();
                    stream.skipWhiteSpaces();
                }

                if (stream.get() != '=') {
                    info += GenerateWarning("Expect to '='", stream.get(), stream.Position);
                    break;
                }
                stream.skip();

                stream.skipWhiteSpaces();

                identName = stream.parseIdentifier();
                if (identName != "function") {
                    info += GenerateWarning("Expect to 'function'", identName, stream.Position);
                    break;
                }
                stream.skipWhiteSpaces();

                if (stream.get() != '(') {
                    info += GenerateWarning("Expect to '('", stream.get(), stream.Position);
                    break;
                }
                stream.skip();

                if (stream.get() != ')') {
                    info += GenerateWarning("Expect to ')'", stream.get(), stream.Position);
                    break;
                }
                stream.skip();

                stream.skipWhiteSpaces();

                if (stream.get() != '{') {
                    info += GenerateWarning("Expect to '{'", stream.get(), stream.Position);
                    break;
                }
                stream.skip();

                stream.skipWhiteSpaces();

                do {
                    if (!stream.hasNext()) {
                        // It's reach the end of code.
                        break;
                    }

                    if (stream.get() == '}') {
                        stream.skip();
                        if (stream.get() != ';') {
                            info += GenerateWarning("Expect to ';'", stream.get(), stream.Position);
                            break;
                        }
                        stream.skip();
                        stream.skipWhiteSpaces();

                        // It's the end of code.
                        break;
                    }

                    identName = stream.parseIdentifier();
                    if (identName != "this") {
                        info += GenerateWarning("Expect to 'this'", stream.get(), stream.Position);
                        break;
                    }

                    stream.skipWhiteSpaces();

                    if (stream.get() != '.') {
                        info += GenerateWarning("Expect to '.'", stream.get(), stream.Position);
                        break;
                    }
                    stream.skip();

                    stream.skipWhiteSpaces();

                    fieldName = stream.parseIdentifier();
                    if (fieldName.Length <= 0) {
                        info += GenerateWarning("Expect to [field name]", stream.get(), stream.Position);
                        break;
                    }

                    stream.skipWhiteSpaces();

                    if (stream.get() != '=') {
                        info += GenerateWarning("Expect to '='", stream.get(), stream.Position);
                        break;
                    }
                    stream.skip();

                    stream.skipWhiteSpaces();

                    fieldValue = stream.parseToDelimiter(";\r\n}");
                    if (fieldValue.Length <= 0) {
                        info += GenerateWarning("Expect to [field value]", stream.get(), stream.Position);
                        break;
                    }
                    stream.skip();

                    stream.skipWhiteSpaces();

                    // Append the fieldName and fieldValue
                    fieldList.Add(new StructField(fieldName, fieldValue));
                } while (true);

                csharpCode = string.Format("struct {0}\r\n{{\r\n", funcName);
                foreach (var field in fieldList) {
                    csharpCode += string.Format("    public long {0} = {1};\r\n", field.name, field.value);
                }
                csharpCode += string.Format("}};");
            } while (false);

            infos = info;
            return csharpCode;
        }
    }
}
