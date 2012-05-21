using System;

namespace LHASocialWork.Entities.Enumerations
{
    [Flags]
    public enum SystemRole
    {
        Member,
        Volunteer,
        Staff,
        Donor,
        Admin
    }
}