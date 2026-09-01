namespace NursingHome.Domain.Enums;

public enum UserStatus
{
    // conflict in BA doc, on user story call with ENABLE but seed db is ACTIVE :D
    INACTIVE  = 0,
    ACTIVE = 1,
    LOCKED = 2,
}