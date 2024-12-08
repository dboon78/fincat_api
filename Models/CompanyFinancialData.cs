using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("CompanyFinancialData")]
    public class CompanyFinancialData
    {
        public int FinDataId { get; set; } // Unique identifier for database purposes

        // Metadata
        public string Symbol{get; set; }
        public double Price{ get; set; }  //summary  
        public string CEO { get; set; } // Current CEO (from Income Statement table)
        public string ReportedCurrency { get; set; } // Reported currency (from Income Statement table)
        public int Year { get; set; } // Financial year (from financial report data)

        // Profitability Metrics
        public double NetIncome { get; set; } // Income Statement table - Net income for the period
        public double FreeCashFlow { get; set; } // Cash Flow table - Total free cash flow
        public double OperatingCashFlow { get; set; } // FMPCashFlow table - Operating cash flow analysis
        public double DepreciationAndAmortization { get; set; } // FMPCashFlow table - Non-cash adjustments

        // Cash Flow Activities
        public double NetCashProvidedByOperatingActivities { get; set; } // FMPCashFlow table (cash flow from operations)
        public double NetCashUsedForInvestingActivities { get; set; } // FMPCashFlow table (investing activities)
        public double NetCashUsedProvidedByFinancingActivities { get; set; } // FMPCashFlow table (financing activities)
        public double EffectOfForexChangesOnCash { get; set; } // FMPCashFlow table (forex changes on cash)
        public double NetChangeInCash { get; set; } // FMPCashFlow table (net change in cash)
        public double CashAtBeginningOfPeriod { get; set; } // FMPCashFlow table (beginning cash balance)
        public double CashAtEndOfPeriod { get; set; } // FMPCashFlow table (ending cash balance)

        // Investments
        public double InvestmentsInPropertyPlantAndEquipment { get; set; } // FMPCashFlow table (capital investments)
        public double PurchasesOfInvestments { get; set; } // FMPCashFlow table (investment purchases)
        public double SalesMaturitiesOfInvestments { get; set; } // FMPCashFlow table (investment maturity sales)

        // Financing Activities
        public double DebtRepayment { get; set; } // Cash Flow table (repayment of debt)
        public double CommonStockIssued { get; set; } // Cash Flow table (issuance of common stock)
        public double CommonStockRepurchased { get; set; } // Cash Flow table (share repurchases)
        public double DividendsPaid { get; set; } // Cash Flow table (dividends paid to shareholders)

        // Calculated Metrics
        public double FreeCashFlowMargin => Revenue > 0 ? (FreeCashFlow / Revenue) * 100 : 0; // Derived using Revenue
        public double CashFlowToDebtRatio => TotalDebt > 0 ? OperatingCashFlow / TotalDebt : 0; // Derived using TotalDebt

        // Additional Financial Data
        public double Revenue { get; set; } // Revenue from financial Income Statement data
        public double TotalDebt { get; set; } // Total debt data from Balance Sheet table
        public double TotalAssets { get; set; } // Total assets from Balance Sheet table
        public double TotalLiabilities { get; set; } // Total liabilities from Balance Sheet table
        public double Equity { get; set; } // Equity calculated from Balance Sheet (Assets - Liabilities)

        // Metadata
        public DateTime CreatedAt { get; set; } // Timestamp for financial data insertion
        public string SourceLink { get; set; } // Link to the original financial report source
    }
}