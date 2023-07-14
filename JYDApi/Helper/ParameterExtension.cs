namespace JYD.Helper
{
    public static class ParameterExtension
    {
        const string STRING = "ST";
        const string NUMBER = "NM";
        const string DECIMAL = "DC";
        const string BOOLEAN = "BL";
        const string DATETIME = "DT";

        public static dynamic? GetParameterValue(IEnumerable<RequestParameter> query, string parameterName)
        {

            var item = query.Where(r => r.ParameterName == parameterName).FirstOrDefault();

            if (item == null)
            {
                return null;
            }

            switch (item.ParameterType)
            {
                case STRING: return item.ParameterValue;
                case NUMBER: return Convert.ToInt32(string.IsNullOrEmpty(item.ParameterValue) ? "0" : item.ParameterValue);
                case DECIMAL: return Convert.ToDecimal(string.IsNullOrEmpty(item.ParameterValue) ? "0.00" : item.ParameterValue);
                case DATETIME: return Convert.ToDateTime(string.IsNullOrEmpty(item.ParameterValue) ? string.Empty : item.ParameterValue);
                case BOOLEAN:
                    return (
                            item.ParameterValue.Trim() == "1" ||
                            item.ParameterValue.Trim().ToUpper() == "YES" ||
                            item.ParameterValue.Trim().ToUpper() == "Y" ||
                            item.ParameterValue.Trim().ToUpper() == "TRUE")
                             ? true : false;
                default:
                    return (dynamic)item.ParameterValue;
            }
        }
    }
}
