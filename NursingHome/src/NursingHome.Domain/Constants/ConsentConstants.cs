namespace NursingHome.Domain.Constant;

public static class ConsentConstants
{
    public static readonly List<string> RequiredConsents = new()
    {
        "Admission Agreement",
        "HIPAA Notice of Privacy Practices",
        "Financial Responsibility Agreement",
        "Advance Directive Acknowledgment",
        "Arbitration Agreement"
    };
}
