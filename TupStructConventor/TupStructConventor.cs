using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TarsTupHelper
{
    public enum VarType
    {
        Unknown,
        Bool,
        Char,
        Short,
        Int,
        Long,
        Enum,
        Float,
        Double,
        String,
        Vector,
        Map,
        Struct,
        Object,
        ByteArray,
        Last
    }

    public class MemberVariable
    {
        private VarType vt = VarType.Unknown;
        private string type = "";
        private string name = "";
        private string value = "";

        public MemberVariable()
        {
        }

        public MemberVariable(VarType vt, string type, string name, string value)
        {
            setVariable(vt, type, name, value);
        }

        public void setVariable(VarType vt, string type, string name, string value)
        {
            this.vt = vt;
            this.type = type;
            this.name = name;
            this.value = value;
        }

        public VarType Vt
        {
            get { return vt; }
            set { vt = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    public class TupStructConventor
    {
        private string jsCode = "";
        private string csharpCode = "";
        private string infos = "";

        private static string codeTemplate =
@"    public class [$$ClassName$$] : TarsStruct
    {
        [$$Declaration$$]

        public [$$ClassName$$]()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            [$$ReadFrom$$]
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            [$$WriteTo$$]
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            [$$Display$$]
        }
    }";

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

        public static string translateVarType(string typeName)
        {
            switch (typeName) {
                case "ENUM":
                    return "int";

                case "Bool":
                case "Boolean":
                case "BOOLEAN":
                    return "bool";

                case "CHAR":
                    return "char";

                case "SHORT":
                    return "short";

                case "INT32":
                    return "int";

                case "INT64":
                    return "long";

                case "BinBuffer":
                    return "byte[]";

                case "Float":
                case "FLOAT":
                    return "float";

                case "Double":
                case "DOUBLE":
                    return "double";

                case "STRING":
                    return "string";

                default:
                    return typeName;
            }
        }

        public static bool parseStructName(string value, ref string _structName)
        {
            bool correct = false;
            // "new HUYA.UserId;"
            const string pattern = @"new (.+)\.(.+)";
            Regex reg = new Regex(pattern);
            Match match = reg.Match(value);

            if (match.Groups.Count == 3) {
                string structNS = match.Groups[1].Value;
                string structName = match.Groups[2].Value;
                _structName = structName;
                correct = true;
            }
            return correct;
        }

        public static bool parseVectorName(string value, ref string _vectorName)
        {
            bool correct = false;
            // "new Taf.Vector(new HUYA.DecorationInfo);"
            const string pattern = @"new (.+)\.(.+)\(new (.+)\.(.+)\)";
            Regex reg = new Regex(pattern);
            Match match = reg.Match(value);

            if (match.Groups.Count == 5) {
                string vectorType = match.Groups[2].Value;
                string vectorName = match.Groups[4].Value;
                if (vectorType == "Vector") {
                    vectorName = translateVarType(vectorName);
                    _vectorName = vectorName;
                    correct = true;
                }
            }
            return correct;
        }

        public static bool parseMapName(string value, ref string _mapName)
        {
            bool correct = false;
            // "new Taf.Map(new Taf.STRING, new Taf.STRING);"
            const string pattern1 = @"new (.+)\.(.+)\((.+)\)";
            const string pattern2 = @"new (.+)\.(.+)";
            Regex reg = new Regex(pattern1);
            Match match = reg.Match(value);

            if (match.Groups.Count == 4) {
                string mapType = match.Groups[2].Value;
                string mapNameList = match.Groups[3].Value;
                if (mapType == "Map") {
                    string[] nameList = mapNameList.Split(',');
                    ArrayList typeNameList = new ArrayList();
                    Regex reg2 = new Regex(pattern2);

                    foreach (var name in nameList) {
                        string tName = name.Trim();
                        Match match2 = reg2.Match(tName);
                        if (match2.Groups.Count == 3) {
                            string typeName = match2.Groups[2].Value;
                            typeName = translateVarType(typeName);
                            typeNameList.Add(typeName);
                        }
                    }

                    string mapName = "";
                    int i = 0;
                    foreach (var typeName in typeNameList) {
                        if (i < typeNameList.Count - 1)
                            mapName += string.Format("{0}, ", typeName);
                        else
                            mapName += string.Format("{0}", typeName);
                        i++;
                    }

                    _mapName = mapName;
                    correct = true;
                }
            }
            return correct;
        }

        public static bool verifyFieldType(VarType vt, string fieldValue, ref string value)
        {
            bool correct = false;
            switch (vt) {
                case VarType.Bool: {
                        bool bvalue = false;
                        correct = Chars.parseBoolean(fieldValue, ref bvalue);
                        if (correct) {
                            value = bvalue ? "true" : "false";
                        }
                        break;
                    }

                case VarType.Short:
                case VarType.Int:
                case VarType.Long:
                    {
                        long lvalue = 0;
                        correct = Chars.parseInteger(fieldValue, ref lvalue);
                        break;
                    }

                case VarType.Enum:
                    {
                        correct = true;
                        break;
                    }

                case VarType.String:
                    {
                        string svalue = "";
                        correct = Chars.parseString(fieldValue, ref svalue);
                        break;
                    }

                default:
                    break;
            }
            return correct;
        }

        public static bool parseFieldType(string fieldName, ref string fieldValue,
                                          ref VarType type, ref string fieldType)
        {
            bool correct = false;
            string varType = "";

            if (string.IsNullOrEmpty(fieldName))
                return false;

            char cType = fieldName[0];
            switch (cType) {
                case 'b':
                    type = VarType.Bool;
                    correct = verifyFieldType(VarType.Bool, fieldValue, ref varType);
                    if (correct) {
                        fieldType = "bool";
                        fieldValue = varType;
                    }
                    break;

                case 'c':
                    type = VarType.Char;
                    varType = "char";
                    correct = verifyFieldType(VarType.Char, fieldValue, ref varType);
                    if (correct)
                        fieldType = "char";
                    break;

                case 'i':
                    type = VarType.Int;
                    correct = verifyFieldType(VarType.Int, fieldValue, ref varType);
                    if (correct)
                        fieldType = "int";
                    break;

                case 'l':
                    type = VarType.Long;
                    correct = verifyFieldType(VarType.Long, fieldValue, ref varType);
                    if (correct)
                        fieldType = "long";
                    break;

                case 'e':
                    type = VarType.Enum;
                    correct = verifyFieldType(VarType.Enum, fieldValue, ref varType);
                    if (correct)
                        fieldType = "int";
                    break;

                case 's':
                    type = VarType.String;
                    correct = verifyFieldType(VarType.String, fieldValue, ref varType);
                    if (correct)
                        fieldType = "string";
                    break;

                case 't':
                    type = VarType.Struct;
                    correct = parseStructName(fieldValue, ref varType);
                    if (correct) {
                        fieldType = varType;
                        fieldValue = string.Format("new {0}()", varType);
                    }
                    break;

                case 'v':
                    type = VarType.Vector;
                    correct = parseVectorName(fieldValue, ref varType);
                    if (correct) {
                        fieldType = string.Format("List<{0}>", varType);
                        fieldValue = string.Format("new List<{0}>()", varType);
                    }
                    break;

                case 'm':
                    type = VarType.Map;
                    correct = parseMapName(fieldValue, ref varType);
                    if (correct) {
                        fieldType = string.Format("Dictionary<{0}>", varType);
                        fieldValue = string.Format("new Dictionary<{0}>()", varType);
                    }
                    break;

                default:
                    fieldType = fieldValue;
                    correct = true;
                    break;
            }

            return correct;
        }

        public static string ConventToCSharp(string jsCode, ref string infos)
        {
            string csharpCode = "";
            string info = "";
            const string HUYA_Prefix = "HUYA";

            StringStream stream = new StringStream(jsCode);

            do {
                string structName;
                VarType type;
                string fieldType;
                string fieldName;
                string fieldValue;
                string identName;

                List<MemberVariable> fieldList = new List<MemberVariable>();

                stream.skipWhiteSpaces();

                structName = stream.parseIdentifier();
                stream.skipWhiteSpaces();

                if (structName == HUYA_Prefix) {
                    //info += "It's a HUYA Live class.\r\n";
                }

                while (stream.get() == '.') {
                    // Skip the '.' char inside struct name .
                    stream.skip();
                    structName = stream.parseIdentifier();
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

                    if (stream.get() == ';') {
                        stream.skip();
                    }

                    stream.skipWhiteSpaces();

                    type = VarType.Unknown;
                    fieldType = "";
                    bool correct = parseFieldType(fieldName, ref fieldValue, ref type, ref fieldType);

                    // Append the fieldName and fieldValue
                    fieldList.Add(new MemberVariable(type, fieldType, fieldName, fieldValue));

                } while (true);

                string declaration = "";
                int index = 0;
                foreach (var field in fieldList) {
                    if (index == 0) {
                        declaration += string.Format("public {0} {1} = {2};",
                                                     field.Type, field.Name, field.Value);
                    }
                    else {
                        declaration += string.Format("        public {0} {1} = {2};",
                                                     field.Type, field.Name, field.Value);
                    }
                    if (index < fieldList.Count - 1) {
                        declaration += "\r\n";
                    }
                    index++;
                }

                string readFrom = "";
                index = 0;
                foreach (var field in fieldList) {
                    if (index == 0) {
                        readFrom += string.Format("{0} = ({1})_is.Read({2}, {3}, false);",
                                                  field.Name, field.Type, field.Name, index);
                    }
                    else {
                        readFrom += string.Format("            {0} = ({1})_is.Read({2}, {3}, false);",
                                                  field.Name, field.Type, field.Name, index);
                    }
                    if (index < fieldList.Count - 1) {
                        readFrom += "\r\n";
                    }
                    index++;
                }

                string writeTo = "";
                index = 0;
                foreach (var field in fieldList) {
                    if (index == 0) {
                        writeTo += string.Format("_os.Write({0}, {1});",
                                                 field.Name, index);
                    }
                    else {
                        writeTo += string.Format("            _os.Write({0}, {1});",
                                                 field.Name, index);
                    }
                    if (index < fieldList.Count - 1) {
                        writeTo += "\r\n";
                    }
                    index++;
                }

                string display = "";
                index = 0;
                foreach (var field in fieldList) {
                    if (index == 0) {
                        display += string.Format("_ds.Display({0}, \"{1}\");",
                                                 field.Name, field.Name);
                    }
                    else {
                        display += string.Format("            _ds.Display({0}, \"{1}\");",
                                                 field.Name, field.Name);
                    }
                    if (index < fieldList.Count - 1) {
                        display += "\r\n";
                    }
                    index++;
                }

                string outputCode = TupStructConventor.codeTemplate;

                outputCode = outputCode.Replace("[$$ClassName$$]", structName);
                outputCode = outputCode.Replace("[$$Declaration$$]", declaration);
                outputCode = outputCode.Replace("[$$ReadFrom$$]", readFrom);
                outputCode = outputCode.Replace("[$$WriteTo$$]", writeTo);
                outputCode = outputCode.Replace("[$$Display$$]", display);

                csharpCode = outputCode;
            } while (false);

            infos = info;
            return csharpCode;
        }
    }
}
