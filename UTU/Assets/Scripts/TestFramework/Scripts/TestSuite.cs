
public class TestSuite {

    #region CONSTRUCTOR

    public TestSuite (string name) {
        m_name = name;
        m_list = new System.Collections.Generic.List<TestCase>();
    }

    #endregion


    #region PUBLIC
    public int Count { get { return m_list.Count; } }

    public void Add (TestCase testCase) {
        m_list.Add(testCase);
    }

    public string GetName () {
        return m_name;
    }
    public TestResult Run (TestResult result) {
        m_result = result;
        m_list.ForEach(RunCase);
        result.SetTestName(m_name);
        return result;
    }
    #endregion

    private void RunCase (TestCase testCase) {
        testCase.Run(m_result);
    }


    #region PRIVATE

    string m_name;
    TestResult m_result;
    System.Collections.Generic.List<TestCase> m_list;

    #endregion
}
