public class DummyTestCase: TestCase {

    #region CONSTRUCTOR

    public DummyTestCase (string methodName) : base(methodName) {
        m_shouldSetupFail = false;
    }

    #endregion


    #region PUBLIC

    public string StateLog = "";


    public void SetSetupFail (bool shouldFail) {
        m_shouldSetupFail = shouldFail;
    }

    override public void Setup () {
        if (m_shouldSetupFail) {
            throw new System.Exception();
        }
        StateLog = "Setup ";
    }

    public void DummyPassingTest () {
        StateLog += "Run ";
    }

    override public void Teardown () {
        StateLog += "Teardown";
    }

    #endregion


    #region PRIVATE

    bool m_shouldSetupFail;

    #endregion
}
