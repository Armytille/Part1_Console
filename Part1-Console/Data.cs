namespace Part1_Console
{
    public class Data
    {
        public int LineNumber;
        public string Type;
        public string ConcatAb;
        public int SumCd;

        public Data(int lineNumber, string type, string concatAb, int sumCd)
        {
            LineNumber = lineNumber;
            Type = type;
            ConcatAb = concatAb;
            SumCd = sumCd;
        }
        public Data()
        {
            //To prevent XmlConverter Serialize method from crashing
        }

        public override string ToString()
        {
            return $"lineNumber : {LineNumber}, type : {Type}, concatAB : {ConcatAb}, sumCD: {SumCd}";
        }
    }

    public class OnErrorData
    {
        public int LineNumber;
        public string Type;
        public string ErrorMessage;

        public OnErrorData(int lineNumber, string type, string errorMessage)
        {
            LineNumber = lineNumber;
            Type = type;
            ErrorMessage = errorMessage;
        }

        public OnErrorData()
        {
            //To prevent XmlConverter Serialize method from crashing
        }

        public override string ToString()
        {
            return $"lineNumber : {LineNumber}, type : {Type}, errorMessage : {ErrorMessage}";
        }
    }
}

