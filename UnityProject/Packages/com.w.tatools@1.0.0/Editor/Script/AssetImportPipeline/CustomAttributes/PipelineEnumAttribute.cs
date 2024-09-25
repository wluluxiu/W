namespace jj.TATools.Editor
{
    internal class PipelineEnumAttribute : System.Attribute
    {
        private string m_DisplayName;
        public string DisplayName { get { return m_DisplayName; } }

        public PipelineEnumAttribute(string displayName)
        {
            this.m_DisplayName = displayName;
        }
    }
}


