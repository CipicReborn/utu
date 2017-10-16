
public class TestResult {

    #region CONSTRUCTOR

    public TestResult () {
        m_testName = "";
        m_isSetupSuccessful = false;
        m_isTeardownSuccessful = false;
        m_testsRunCount = 0;
        m_testsFailedCount = 0;
        m_detailedResults = new System.Collections.Generic.List<string>();
    }

    #endregion


    #region PUBLIC

    public void SetTestName (string testName) {
        m_testName = testName;
    }

    public void StartTest () {
        m_testsRunCount += 1;
        m_detailedResults.Add(m_testName + " : Passed");
    }

    public void TestFailed () {
        m_testsFailedCount += 1;
        m_detailedResults[m_detailedResults.Count - 1] = m_testName + " : Failed";
    }

    public int GetTotalTestsCount () {
        return m_testsRunCount;
    }

    public int GetPassingTestsCount () {
        return m_testsRunCount - m_testsFailedCount;
    }

    public int GetFailingTestsCount () {
        return m_testsFailedCount;
    }

    public string Summary () {
        return m_testName + " : " + m_errorMessage + m_testsRunCount.ToString() + " run, " + m_testsFailedCount.ToString() + " failed";
    }

    public System.Collections.Generic.List<string> GetDetails () {
        return m_detailedResults;
    }

    public bool IsSetupSuccessful {
        get { return m_isSetupSuccessful; }
        set {
            m_isSetupSuccessful = value;
            if (value) {
                m_errorMessage = "";
            }
            else {
                m_errorMessage = "[Error] Setup Failed. ";
                m_detailedResults.Add(m_testName + " : " + m_errorMessage);
            }
        }
    }

    public bool IsTeardownSuccessful {
        get { return m_isTeardownSuccessful; }
        set { m_isTeardownSuccessful = value; }
    }

    #endregion


    #region PRIVATE

    private string m_testName;
    private bool m_isSetupSuccessful;
    private bool m_isTeardownSuccessful;
    private int m_testsRunCount;
    private int m_testsFailedCount;
    private string m_errorMessage;
    private System.Collections.Generic.List<string> m_detailedResults;

    #endregion


}
