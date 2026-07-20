using System.Collections.Generic;

namespace NursingHome.Application.Features.BillingInsurance.DTOs;

public record CostBreakdownDto(//Costbreakdown from UI SC_035_M2-US-09_cost-billing-panel
    string Category,// this is COST ITEM from UI
    decimal Amount,// this is RATE/DAY from UI
    string SourceDescription// this is SOURCE from UI
);