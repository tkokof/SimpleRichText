// desc rich syntax parser implementation
// maintainer hugoyu

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace RichText
{

    public class RichSyntaxData
    {

        public RichSyntaxData(string name = null)
        {
            m_name = name;
        }

        public string GetName()
        {
            return m_name;
        }

        public void SetName(string name)
        {
            m_name = name;
        }

        public void AddParam(string paramName, string paramValue)
        {
            m_params.Add(paramName, paramValue);
        }

        public void RemoveParam(string paramName)
        {
            m_params.Remove(paramName);
        }

        public void ClearParams()
        {
            m_params.Clear();
        }

        public bool HasParam(string paramName)
        {
            return m_params.ContainsKey(paramName);
        }

        public string GetParam(string paramName)
        {
            if (m_params.ContainsKey(paramName))
            {
                return m_params[paramName];
            }

            return null;
        }

        public void SetParam(string paramName, string paramValue)
        {
            m_params[paramName] = paramValue;
        }

        public Dictionary<string, string> GetAllParams()
        {
            return new Dictionary<string, string>(m_params);
        }

        // useful interface

        public bool GetParamBoolean(string paramName, bool defaultValue = false)
        {
            var paramValue = GetParam(paramName);
            if (!string.IsNullOrEmpty(paramValue))
            {
                bool boolValue = default(bool);
                if (bool.TryParse(paramValue, out boolValue))
                {
                    return boolValue;
                }
            }

            // return default value
            return defaultValue;
        }

        public int GetParamInt(string paramName, int defaultValue = 0)
        {
            var paramValue = GetParam(paramName);
            if (!string.IsNullOrEmpty(paramValue))
            {
                int intValue = default(int);
                if (int.TryParse(paramValue, out intValue))
                {
                    return intValue;
                }
            }

            return defaultValue;
        }

        public float GetParamFloat(string paramName, float defaultValue = 0)
        {
            var paramValue = GetParam(paramName);
            if (!string.IsNullOrEmpty(paramValue))
            {
                float floatValue = default(float);
                if (float.TryParse(paramValue, out floatValue))
                {
                    return floatValue;
                }
            }

            return defaultValue;
        }

        public string GetParamString(string paramName)
        {
            return GetParam(paramName);
        }

        public Vector2 GetParamVector2(string paramName)
        {
            var paramValue = GetParam(paramName);
            if (!string.IsNullOrEmpty(paramValue))
            {
                var values = paramValue.Split(',');
                if (values.Length >= 2)
                {
                    Vector2 value = new Vector2();
                    float.TryParse(values[0], out value.x);
                    float.TryParse(values[1], out value.y);
                    return value;
                }
                else
                {
                    Debug.LogError("[RichSyntaxParser]Error to get vector2 param : " + paramValue);
                }
            }

            return Vector2.zero;
        }

        public Vector3 GetParamVector3(string paramName)
        {
            var paramValue = GetParam(paramName);
            if (!string.IsNullOrEmpty(paramValue))
            {
                var values = paramValue.Split(',');
                if (values.Length >= 3)
                {
                    Vector3 value = new Vector3();
                    float.TryParse(values[0], out value.x);
                    float.TryParse(values[1], out value.y);
                    float.TryParse(values[2], out value.z);
                    return value;
                }
                else
                {
                    Debug.LogError("[RichSyntaxParser]Error to get vector3 param : " + paramValue);
                }
            }

            return Vector3.zero;
        }

        public Vector4 GetParamVector4(string paramName)
        {
            var paramValue = GetParam(paramName);
            if (!string.IsNullOrEmpty(paramValue))
            {
                var values = paramValue.Split(',');
                if (values.Length >= 4)
                {
                    Vector4 value = new Vector4();
                    float.TryParse(values[0], out value.x);
                    float.TryParse(values[1], out value.y);
                    float.TryParse(values[2], out value.z);
                    float.TryParse(values[3], out value.w);
                    return value;
                }
                else
                {
                    Debug.LogError("[RichSyntaxParser]Error to get vector4 param : " + paramValue);
                }
            }

            return Vector4.zero;
        }

        public bool IsValid()
        {
            return m_name != null;
        }

        public override string ToString()
        {
            var strBuffer = new StringBuilder();
            strBuffer.Append(m_name + " = \n{\n");
            
            var iter = m_params.GetEnumerator();
            while (iter.MoveNext())
            {
                var paramName = iter.Current.Key;
                var paramValue = iter.Current.Value;

                strBuffer.Append("    " + paramName + " = " + paramValue + "\n");
            }

            strBuffer.Append("}");

            return strBuffer.ToString();
        }

        string m_name;
        Dictionary<string, string> m_params = new Dictionary<string, string>();

    }

    public class RichSyntaxParser
    {

        protected char m_syntaxBeginChar = '<';
        protected char m_syntaxEndChar = '>';
        protected char m_syntaxSeparatorChar = '|';
        protected char m_syntaxAssignChar = '=';
        protected char m_syntaxTrimChar = ' ';

        // not so sure about this ...
        // TODO improve
        protected RichText m_richText;

        public virtual char GetSyntaxBeginChar()
        {
            return m_syntaxBeginChar;
        }

        public virtual void SetSyntaxBeginChar(char syntaxBeginChar)
        {
            m_syntaxBeginChar = syntaxBeginChar;
        }

        public virtual bool IsSyntaxBeginChar(char chr)
        {
            return chr == m_syntaxBeginChar;
        }

        public virtual char GetSyntaxEndChar()
        {
            return m_syntaxEndChar;
        }

        public virtual void SetSyntaxEndChar(char syntaxEndChar)
        {
            m_syntaxEndChar = syntaxEndChar;
        }

        public virtual bool IsSyntaxEndChar(char chr)
        {
            return chr == m_syntaxEndChar;
        }

        public virtual char GetSyntaxSeparatorChar()
        {
            return m_syntaxSeparatorChar;
        }
        
        public virtual void SetSyntaxSeparatorChar(char syntaxSeparatorChar)
        {
            m_syntaxSeparatorChar = syntaxSeparatorChar;
        }

        public virtual bool IsSyntaxSeparatorChar(char chr)
        {
            return chr == m_syntaxSeparatorChar;
        }

        public virtual char GetSyntaxAssignChar()
        {
            return m_syntaxAssignChar;
        }

        public virtual void SetSyntaxAssignChar(char syntaxAssignChar)
        {
            m_syntaxAssignChar = syntaxAssignChar;
        }

        public virtual bool IsSyntaxAssignChar(char chr)
        {
            return chr == m_syntaxAssignChar;
        }

        public virtual char GetSyntaxTrimChar()
        {
            return m_syntaxTrimChar;
        }

        public virtual void SetSyntaxTrimChar(char syntaxTrimChar)
        {
            m_syntaxTrimChar = syntaxTrimChar;
        }

        public virtual bool IsSyntaxTrimChar(char chr)
        {
            return chr == m_syntaxTrimChar;
        }

        public virtual bool IsSyntaxChar(char chr)
        {
            return IsSyntaxBeginChar(chr) ||
                   IsSyntaxEndChar(chr) ||
                   IsSyntaxSeparatorChar(chr) ||
                   IsSyntaxAssignChar(chr);
        }
        
        public virtual bool IsTextChar(char chr)
        {
            return !IsSyntaxChar(chr);
        }

        protected virtual int ParseIdentity(string config, int startIndex)
        {
            while (startIndex < config.Length)
            {
                if (IsTextChar(config[startIndex]))
                {
                    ++startIndex;
                }
                else
                {
                    break;
                }
            }

            return startIndex;
        }

        protected virtual int ParseSyntax(string config, int startIndex)
        {
            if (!IsSyntaxBeginChar(config[startIndex]))
            {
                Debug.LogError("[RichSyntaxParser]Unexpected syntax char expected : " + GetSyntaxBeginChar() + " current : " + config[startIndex] + " at index : " + startIndex);
                // return -1 as error occur
                return -1;
            }

            RichSyntaxData data = new RichSyntaxData();

            int nextIndex = startIndex + 1;
            while (nextIndex < config.Length)
            {
                // first parse syntax name
                var nameStartIndex = nextIndex;
                nextIndex = ParseIdentity(config, nextIndex);
                var name = config.Substring(nameStartIndex, nextIndex - nameStartIndex);
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogError("[RichSyntaxParser]Unexpected syntax name at index " + nextIndex);
                    // return -1 as error occur
                    return -1;
                }
                data.SetName(name.Trim(GetSyntaxTrimChar()));

                // then parse syntax params
                while (nextIndex < config.Length)
                {
                    if (IsSyntaxEndChar(config[nextIndex]))
                    {
                        break;
                    }
                    else
                    {
                        if (!IsSyntaxSeparatorChar(config[nextIndex]))
                        {
                            Debug.LogError("[RichSyntaxParser]Unexpected syntax char expected : " + GetSyntaxSeparatorChar() + " current : " + config[nextIndex] + " at index : " + nextIndex);
                            // return -1 as error occur
                            return -1;
                        }
                        ++nextIndex;
                        var paramStartIndex = nextIndex;
                        nextIndex = ParseIdentity(config, nextIndex);
                        var paramName = config.Substring(paramStartIndex, nextIndex - paramStartIndex);
                        if (string.IsNullOrEmpty(paramName))
                        {
                            Debug.LogError("[RichSyntaxParser]Unexpected syntax param name at index " + paramStartIndex);
                            // return -1 as error occur
                            return -1;
                        }
                        if (nextIndex >= config.Length || !IsSyntaxAssignChar(config[nextIndex]))
                        {
                            if (nextIndex < config.Length)
                            {
                                Debug.LogError("[RichSyntaxParser]Unexpected syntax char expected : " + GetSyntaxAssignChar() + " current : " + config[nextIndex] + " at index : " + nextIndex);
                            }
                            else
                            {
                                Debug.LogError("[RichSyntaxParser]Unexpected syntax char expected : " + GetSyntaxAssignChar() + " current is missing at the end of config string ...");
                            }
                            // return -1 as error occur
                            return -1;
                        }
                        ++nextIndex;
                        paramStartIndex = nextIndex;
                        nextIndex = ParseIdentity(config, nextIndex);
                        var paramValue = config.Substring(paramStartIndex, nextIndex - paramStartIndex);
                        if (string.IsNullOrEmpty(paramValue))
                        {
                            Debug.LogError("[RichSyntaxParser]Unexpected syntax param value at index " + paramStartIndex);
                            // return -1 as error occur
                            return -1;
                        };
                        
                        data.AddParam(paramName.Trim(GetSyntaxTrimChar()), paramValue.Trim(GetSyntaxTrimChar()));
                    }
                }

                // check next index end char here
                if (nextIndex >= config.Length || !IsSyntaxEndChar(config[nextIndex]))
                {
                    if (nextIndex < config.Length)
                    {
                        Debug.LogError("[RichSyntaxParser]Unexpected syntax char expected : " + GetSyntaxEndChar() + " current : " + config[nextIndex] + " at index : " + nextIndex);
                    }
                    else
                    {
                        Debug.LogError("[RichSyntaxParser]Unexpected syntax char expected : " + GetSyntaxEndChar() + " current is missing at the end of the config string ...");
                    }
                    // return -1 as error occur
                    return -1;
                }
                else
                {
                    ++nextIndex;
                    break;
                }
            }

            if (data.IsValid())
            {
                OnSyntax(data);
            }

            return nextIndex;
        }

        protected virtual int ParseText(string config, int startIndex)
        {
            if (!IsTextChar(config[startIndex]))
            {
                Debug.LogError("[RichSyntaxParser]Unexpected text at index " + startIndex);
                // return -1 as error occur
                return -1;
            }

            int nextIndex = startIndex + 1;
            while (nextIndex < config.Length)
            {
                if (IsTextChar(config[nextIndex]))
                {
                    ++nextIndex;
                }
                else
                {
                    break;
                }
            }

            var text = config.Substring(startIndex, nextIndex - startIndex);
            Debug.Assert(!string.IsNullOrEmpty(text));
            OnText(text);

            return nextIndex;
        }

        protected virtual bool Parse(string config, int startIndex)
        {
            if (startIndex < 0)
            {
                // negative startIndex means error
                return false;
            }
            else if (startIndex < config.Length)
            {
                if (IsSyntaxChar(config[startIndex]))
                {
                    return Parse(config, ParseSyntax(config, startIndex));
                }
                else
                {
                    return Parse(config, ParseText(config, startIndex));
                }
            }

            // default return true
            return true;
        }

        public virtual bool Parse(string config, RichText richText)
        {
            if (richText)
            {
                // reset first
                Reset();

                // buffer rich text
                m_richText = richText;
                // clear rich text
                m_richText.ClearRichElements();

                // do parse then
                OnParseBegin();
                bool ret = Parse(config, 0);
                OnParseEnd();

                // reset rich text
                m_richText = null;

                return ret;
            }

            return false;
        }

        protected virtual void Reset()
        {
            m_richText = null;
        }

        protected virtual void OnParseBegin()
        {
        }

        protected virtual void OnSyntax(RichSyntaxData syntaxData)
        {
        }

        protected virtual void OnText(string text)
        {
        }

        protected virtual void OnParseEnd()
        {
        }

    }

}
