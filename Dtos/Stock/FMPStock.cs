using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;

namespace api.Dtos.Stock
{
    public class FMPIncomeStatement{
        public string Date { get; set; }
        public string Symbol { get; set; }
        public string ReportedCurrency { get; set; }
        public string Cik { get; set; }
        public string FillingDate { get; set; }
        public string AcceptedDate { get; set; }
        public string CalendarYear { get; set; }
        public string Period { get; set; }
        public double Revenue { get; set; }
        public double CostOfRevenue { get; set; }
        public double GrossProfit { get; set; }
        public double GrossProfitRatio { get; set; }
        public double ResearchAndDevelopmentExpenses { get; set; }
        public double GeneralAndAdministrativeExpenses { get; set; }
        public double SellingAndMarketingExpenses { get; set; }
        public double SellingGeneralAndAdministrativeExpenses { get; set; }
        public double OtherExpenses { get; set; }
        public double OperatingExpenses { get; set; }
        public double CostAndExpenses { get; set; }
        public double InterestIncome { get; set; }
        public double InterestExpense { get; set; }
        public double DepreciationAndAmortization { get; set; }
        public double Ebitda { get; set; }
        public double EbitdaRatio { get; set; }
        public double OperatingIncome { get; set; }
        public double OperatingIncomeRatio { get; set; }
        public double TotalOtherIncomeExpensesNet { get; set; }
        public double IncomeBeforeTax { get; set; }
        public double IncomeBeforeTaxRatio { get; set; }
        public double IncomeTaxExpense { get; set; }
        public double NetIncome { get; set; }
        public double NetIncomeRatio { get; set; }
        public double Eps { get; set; }
        public double EpsDiluted { get; set; }
        public double WeightedAverageShsOut { get; set; }
        public double WeightedAverageShsOutDil { get; set; }
        public string Link { get; set; }
        public string FinalLink { get; set; }
    }
    public class FMPKeyRatios{   
         public string symbol { get; set; }
        public string date { get; set; }
        public string calendarYear { get; set; }
        public string period { get; set; }
        public double currentRatio { get; set; }
        public double quickRatio { get; set; }
        public double cashRatio { get; set; }
        public double daysOfSalesOutstanding { get; set; }
        public double daysOfInventoryOutstanding { get; set; }
        public double operatingCycle { get; set; }
        public double daysOfPayablesOutstanding { get; set; }
        public double cashConversionCycle { get; set; }
        public double grossProfitMargin { get; set; }
        public double operatingProfitMargin { get; set; }
        public double pretaxProfitMargin { get; set; }
        public double netProfitMargin { get; set; }
        public double effectiveTaxRate { get; set; }
        public double returnOnAssets { get; set; }
        public double returnOnEquity { get; set; }
        public double returnOnCapitalEmployed { get; set; }
        public double netIncomePerEBT { get; set; }
        public double ebtPerEbit { get; set; }
        public double ebitPerRevenue { get; set; }
        public double debtRatio { get; set; }
        public double debtEquityRatio { get; set; }
        public double longTermDebtToCapitalization { get; set; }
        public double totalDebtToCapitalization { get; set; }
        public double interestCoverage { get; set; }
        public double cashFlowToDebtRatio { get; set; }
        public double companyEquityMultiplier { get; set; }
        public double receivablesTurnover { get; set; }
        public double payablesTurnover { get; set; }
        public double inventoryTurnover { get; set; }
        public double fixedAssetTurnover { get; set; }
        public double assetTurnover { get; set; }
        public double operatingCashFlowPerShare { get; set; }
        public double freeCashFlowPerShare { get; set; }
        public double cashPerShare { get; set; }
        public int payoutRatio { get; set; }
        public double operatingCashFlowSalesRatio { get; set; }
        public double freeCashFlowOperatingCashFlowRatio { get; set; }
        public double cashFlowCoverageRatios { get; set; }
        public double shortTermCoverageRatios { get; set; }
        public double capitalExpenditureCoverageRatio { get; set; }
        public double dividendPaidAndCapexCoverageRatio { get; set; }
        public int dividendPayoutRatio { get; set; }
        public double priceBookValueRatio { get; set; }
        public double priceToBookRatio { get; set; }
        public double priceToSalesRatio { get; set; }
        public double priceEarningsRatio { get; set; }
        public double priceToFreeCashFlowsRatio { get; set; }
        public double priceToOperatingCashFlowsRatio { get; set; }
        public double priceCashFlowRatio { get; set; }
        public double priceEarningsToGrowthRatio { get; set; }
        public double priceSalesRatio { get; set; }
        public int dividendYield { get; set; }
        public double enterpriseValueMultiple { get; set; }
        public double priceFairValue { get; set; }
    }
    
    public class FMPBalanceSheet{
        public string date { get; set; }
        public string symbol { get; set; }
        public string reportedCurrency { get; set; }
        public string cik { get; set; }
        public string fillingDate { get; set; }
        public string acceptedDate { get; set; }
        public string calendarYear { get; set; }
        public string period { get; set; }
        public long cashAndCashEquivalents { get; set; }
        public long shortTermInvestments { get; set; }
        public long cashAndShortTermInvestments { get; set; }
        public double netReceivables { get; set; }
        public long inventory { get; set; }
        public double otherCurrentAssets { get; set; }
        public double totalCurrentAssets { get; set; }
        public long propertyPlantEquipmentNet { get; set; }
        public double goodwill { get; set; }
        public double intangibleAssets { get; set; }
        public double goodwillAndIntangibleAssets { get; set; }
        public double longTermInvestments { get; set; }
        public double taxAssets { get; set; }
        public double otherNonCurrentAssets { get; set; }
        public long totalNonCurrentAssets { get; set; }
        public double otherAssets { get; set; }
        public long totalAssets { get; set; }
        public long accountPayables { get; set; }
        public double shortTermDebt { get; set; }
        public double taxPayables { get; set; }
        public double deferredRevenue { get; set; }
        public long otherCurrentLiabilities { get; set; }
        public long totalCurrentLiabilities { get; set; }
        public long longTermDebt { get; set; }
        public double deferredRevenueNonCurrent { get; set; }
        public double deferredTaxLiabilitiesNonCurrent { get; set; }
        public double otherNonCurrentLiabilities { get; set; }
        public long totalNonCurrentLiabilities { get; set; }
        public double otherLiabilities { get; set; }
        public double capitalLeaseObligations { get; set; }
        public long totalLiabilities { get; set; }
        public double preferredStock { get; set; }
        public double commonStock { get; set; }
        public double retainedEarnings { get; set; }
        public double accumulatedOtherComprehensiveIncomeLoss { get; set; }
        public long othertotalStockholdersEquity { get; set; }
        public long totalStockholdersEquity { get; set; }
        public long totalEquity { get; set; }
        public long totalLiabilitiesAndStockholdersEquity { get; set; }
        public int minorityInterest { get; set; }
        public long totalLiabilitiesAndTotalEquity { get; set; }
        public double totalInvestments { get; set; }
        public long totalDebt { get; set; }
        public long netDebt { get; set; }
        public string link { get; set; }
        public string finalLink { get; set; }
    
    }

    public class FMPCashFlow
    {
        public string Date { get; set; }
        public string Symbol { get; set; }
        public string ReportedCurrency { get; set; }
        public string Cik { get; set; }
        public string FillingDate { get; set; }
        public string AcceptedDate { get; set; }
        public string CalendarYear { get; set; }
        public string Period { get; set; }
        public double NetIncome { get; set; }
        public double DepreciationAndAmortization { get; set; }
        public double DeferredIncomeTax { get; set; }
        public double StockBasedCompensation { get; set; }
        public double ChangeInWorkingCapital { get; set; }
        public double AccountsReceivables { get; set; }
        public double Inventory { get; set; }
        public double AccountsPayables { get; set; }
        public double OtherWorkingCapital { get; set; }
        public double OtherNonCashItems { get; set; }
        public double NetCashProvidedByOperatingActivities { get; set; }
        public double InvestmentsInPropertyPlantAndEquipment { get; set; }
        public double AcquisitionsNet { get; set; }
        public double PurchasesOfInvestments { get; set; }
        public double SalesMaturitiesOfInvestments { get; set; }
        public double OtherInvestingActivities { get; set; }
        public double NetCashUsedForInvestingActivities { get; set; }
        public double DebtRepayment { get; set; }
        public double CommonStockIssued { get; set; }
        public double CommonStockRepurchased { get; set; }
        public double DividendsPaid { get; set; }
        public double OtherFinancingActivities { get; set; }
        public double NetCashUsedProvidedByFinancingActivities { get; set; }
        public double EffectOfForexChangesOnCash { get; set; }
        public double NetChangeInCash { get; set; }
        public double CashAtEndOfPeriod { get; set; }
        public double CashAtBeginningOfPeriod { get; set; }
        public double OperatingCashFlow { get; set; }
        public double CapitalExpenditure { get; set; }
        public double FreeCashFlow { get; set; }
        public string Link { get; set; }
        public string FinalLink { get; set; }
    }
    public class FMPProfile
    {
        public string symbol { get; set; }
        public double price { get; set; }
        public double beta { get; set; }
        public int volAvg { get; set; }
        public long mktCap { get; set; }
        public int lastDiv { get; set; }
        public string range { get; set; }
        public double changes { get; set; }
        public string companyName { get; set; }
        public string currency { get; set; }
        public string cik { get; set; }
        public string isin { get; set; }
        public string cusip { get; set; }
        public string exchange { get; set; }
        public string exchangeShortName { get; set; }
        public string industry { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string ceo { get; set; }
        public string sector { get; set; }
        public string country { get; set; }
        public string fullTimeEmployees { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public double dcfDiff { get; set; }
        public double dcf { get; set; }
        public string image { get; set; }
        public string ipoDate { get; set; }
        public bool defaultImage { get; set; }
        public bool isEtf { get; set; }
        public bool isActivelyTrading { get; set; }
        public bool isAdr { get; set; }
        public bool isFund { get; set; }
    }


    public class FMPSearch{
        public string symbol { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public string stockExchange { get; set; }
        public string exchangeShortName { get; set; }
    }
    public class FMPStock
    {
        public string symbol { get; set; }
        public decimal price { get; set; }
        public double beta { get; set; }
        public int volAvg { get; set; }
        public long mktCap { get; set; }
        public double lastDiv { get; set; }
        public string range { get; set; }
        public double changes { get; set; }
        public string companyName { get; set; }
        public string currency { get; set; }
        public string cik { get; set; }
        public string isin { get; set; }
        public string cusip { get; set; }
        public string exchange { get; set; }
        public string exchangeShortName { get; set; }
        public string industry { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string ceo { get; set; }
        public string sector { get; set; }
        public string country { get; set; }
        public string fullTimeEmployees { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public double dcfDiff { get; set; }
        public double dcf { get; set; }
        public string image { get; set; }
        public string ipoDate { get; set; }
        public bool defaultImage { get; set; }
        public bool isEtf { get; set; }
        public bool isActivelyTrading { get; set; }
        public bool isAdr { get; set; }
        public bool isFund { get; set; }
    }

    public class FMPCrypto{
        public string symbol { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double changesPercentage { get; set; }
        public double change { get; set; }
        public double dayLow { get; set; }
        public double dayHigh { get; set; }
        public double yearHigh { get; set; }
        public double yearLow { get; set; }
        public double marketCap { get; set; }
        public double priceAvg50 { get; set; }
        public double priceAvg200 { get; set; }
        public string exchange { get; set; }
        public long volume { get; set; }
        public long avgVolume { get; set; }
        public double open { get; set; }
        public double previousClose { get; set; }
        public object eps { get; set; }
        public object pe { get; set; }
        public object earningsAnnouncement { get; set; }
        public double sharesOutstanding { get; set; }
        public long timestamp { get; set; }
    }

    public class FMPHistoricPrices{
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public int low { get; set; }
        public double close { get; set; }
        public double adjClose { get; set; }
        public long volume { get; set; }
        public long unadjustedVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double vwap { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }

    }

    public class FMPExchangeHours{
        public string name { get; set; }
        public string openingHour { get; set; }
        public DateTime OpeningHour=>openingHour.UtcTime(timezone);
        public string closingHour { get; set; }
        public DateTime ClosingHour=>closingHour.UtcTime(timezone);
        public string timezone { get; set; }
        public bool isMarketOpen { get; set; }
    }
}