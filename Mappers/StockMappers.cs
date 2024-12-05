using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.PriceChange;
using api.Dtos.Stock;
using api.Extensions;
using api.Models;
using Microsoft.EntityFrameworkCore.Storage;
using static api.Dtos.Stock.TwelveData;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockExchangeDto ToExchangeDto(this StockExchangeDto model){
            return new StockExchangeDto{
                ExchangeName=model.ExchangeName,
                openingHour=model.openingHour.TodayTime(),
                closingHour=model.closingHour.TodayTime(),
                
            };
        }
        public static PriceChangeDto ToPriceChangeDto(this PriceChange priceChange){
            return new PriceChangeDto{
                _1D=priceChange._1D,
                _5D=priceChange._5D,
                _1M=priceChange._1M,
                _3M=priceChange._3M,
                _6M=priceChange._6M,
                ytd=priceChange.ytd,
                _1Y=priceChange._1Y,
                _3Y=priceChange._3Y,
                _5Y=priceChange._5Y,
                _10Y=priceChange._10Y,
                max=priceChange.max
            };
        }
        public static StockDto ToStockDto(this Stock stockModel){
            return new StockDto{
                Id=stockModel.Id,
                Symbol=stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry=stockModel.Industry,
                MarketCap=stockModel.MarketCap,
                Comments=stockModel.Comments.Select(x=>x.ToCommentDto()).ToList(),
                PriceChange=stockModel.PriceChange?.ToPriceChangeDto(),
                Exchange=stockModel.Exchange?.ToExchangeDto(),
            };
        }   
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto){
            return new Stock(){
                Symbol=stockDto.Symbol,
                CompanyName=stockDto.CompanyName,
                Purchase=stockDto.Purchase,
                LastDiv=stockDto.LastDiv,
                Industry=stockDto.Industry,
                MarketCap=stockDto.MarketCap,
                LastUpdated=DateTime.UtcNow
            };   
        }
        public static Stock ToStockFromFMPCrypto(this FMPCrypto crypto){
            return new Stock(){
                Symbol=crypto.symbol,
                CompanyName=crypto.name,
                Purchase=(decimal)crypto.price,
                MarketCap=(long)crypto.marketCap,
                LastUpdated=DateTime.UtcNow,   
            };  
        }
        public static Stock ToStockFromFMP(this FMPStock fmpStock){
            return new Stock(){
                Symbol=fmpStock.symbol,
                CompanyName=fmpStock.companyName,
                Purchase=fmpStock.price,
                LastDiv=(decimal)fmpStock.lastDiv,
                Industry=fmpStock.industry,
                MarketCap=fmpStock.mktCap,
                LastUpdated=DateTime.UtcNow,   
                Logo=fmpStock.image,
                Website=fmpStock.website
            };   
        }
        public static UpdateStockRequestDto ToUpdateDto(this Stock stockModel){
            return new UpdateStockRequestDto(){
                Id=stockModel.Id,
                Symbol=stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry=stockModel.Industry,
                MarketCap=stockModel.MarketCap,
            };
        }
        public static StockExchange ToStockExchange(this FMPExchangeHours exchange){
            return new StockExchange(){
                ExchangeName=exchange.name,
                openingHour=exchange.OpeningHour,
                closingHour=exchange.ClosingHour,    
            };
        }        
        public static StockExchangeDto ToExchangeDto(this StockExchange exchange){
            return new StockExchangeDto(){
                ExchangeName=exchange.ExchangeName,
                openingHour=exchange.openingHour.TodayTime(),
                closingHour=exchange.closingHour.TodayTime()               
            };
        }
        public static HistoricPrice ToHistoricPrice(this CoinApiHistory coinHistory,int stockId){
            return new HistoricPrice{
                High=coinHistory.price_high,
                Low=coinHistory.price_low,
                Close=coinHistory.price_close,
                Open=coinHistory.price_open,
                Volume=coinHistory.volume_traded,
                Date=coinHistory.time_close,
                StockId=stockId
            };
        }
        public static HistoricPrice ToHistoricPrice(this TimeSeriesValue val,string timezone,int stockId){
            return new HistoricPrice{
                High= double.Parse(val.high),
                Low= double.Parse(val.low),
                Close=double.Parse(val.close),
                Open=double.Parse(val.open),
                Volume=long.Parse(val.volume),
                Date=DateTime.ParseExact(val.datetime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                StockId=stockId,
            };
        }
        public static CoinPaprikaDto ToDto(this CoinPaprikaLatest latest,CoinPaprikaCoin coin){
            return new CoinPaprikaDto(){
                name=coin.name,
                symbol=coin.symbol,
                close=latest.close,
                market_cap=latest.market_cap,
            };
        }
        public static Stock ToStock(this CoinPaprikaDto coinDto){
            return new Stock(){
                CompanyName=coinDto.name,
                Symbol=coinDto.symbol,
                Purchase=coinDto.close,
                MarketCap=coinDto.market_cap,
            };
        }
        public static Stock ToStock(this FinnhubProfile finnProfile){
            return new Stock(){
                CompanyName=finnProfile.name,
                MarketCap=finnProfile.marketCapitalization,
                Logo=finnProfile.logo,
                Symbol=finnProfile.ticker,
                Website=finnProfile.weburl
            };
        }
        public static FMPSearch ToFmpSearch(this FinnhubSearch finnsearch){
            return new FMPSearch(){
                symbol=finnsearch.symbol,
                name=finnsearch.description
            };
        }public static FMPSearch ToFmpSearch(this CoinPaprikaCoin coin){
            return new FMPSearch(){
                name=coin.name,
                symbol=coin.symbol,                
            };
        }
    }
}