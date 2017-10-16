
public class TestCase {

    #region CONSTRUCTOR

    public TestCase (string methodName) {
        m_methodName = methodName;
    }

    #endregion


    #region PUBLIC

    virtual public void Setup () { }
    virtual public void Teardown () { }

    public TestResult Run (TestResult result) {
        result.SetTestName(m_methodName);
        try {
            Setup();
            result.IsSetupSuccessful = true;
        }
        catch (System.Exception) {
            result.IsSetupSuccessful = false;
        }

        if (result.IsSetupSuccessful) {
            result.StartTest();
        System.Type thisType = this.GetType();
        System.Reflection.MethodInfo methodToRun = thisType.GetMethod(m_methodName);
        try {
            methodToRun.Invoke(this as object, null);
        }
        catch (System.Exception) {
            result.TestFailed();
        }
        }

        try {
            Teardown();
            result.IsTeardownSuccessful = true;
        }
        catch (System.Exception) {
            result.IsTeardownSuccessful = false;
        }

        return result;
    }

    #endregion


    #region PRIVATE

    protected string m_methodName;

    protected void Assert (bool expression) {
        if (!expression) {
            throw new System.Exception("Failed Test for " + m_methodName);
        }
    }

    #endregion
}
