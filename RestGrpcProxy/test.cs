using Microsoft.AspNetCore.Mvc;

namespace RestGrpcProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherProviderController : ControllerBase
    {

        [HttpGet(Name = "GetAirportWeather")]
        public IActionResult GetAirportWeather()
        {
            return Ok();
        }

        [HttpGet(Name = "GetWeatherForRoute")]
        public IActionResult GetWeatherForRoute()
        {
            return Ok();
        }

        [HttpGet(Name = "GetWeatherForFir")]
        public IActionResult GetWeatherForFir()
        {
            return Ok();
        }

        [HttpGet(Name = "GetWeather")]
        public IActionResult GetWeather()
        {
            return Ok();
        }

        [HttpGet(Name = "GetWeatherVerticalProfile")]
        public IActionResult GetWeatherVerticalProfile()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileConvection")]
        public IActionResult GetVerticalProfileConvection()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileJetStream")]
        public IActionResult GetVerticalProfileJetStream()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileJetStreamPointData")]
        public IActionResult GetVerticalProfileJetStreamPointData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileRadiation")]
        public IActionResult GetVerticalProfileRadiation()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileStorm")]
        public IActionResult GetVerticalProfileStorm()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileSurface")]
        public IActionResult GetVerticalProfileSurface()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileTropopause")]
        public IActionResult GetVerticalProfileTropopause()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileTurbulence")]
        public IActionResult GetVerticalProfileTurbulence()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileTurbulencePointData")]
        public IActionResult GetVerticalProfileTurbulencePointData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileUpperAirWindTemperatures")]
        public IActionResult GetVerticalProfileUpperAirWindTemperatures()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileVolcano")]
        public IActionResult GetVerticalProfileVolcano()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileVulcanicAsh")]
        public IActionResult GetVerticalProfileVulcanicAsh()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVerticalProfileTropicalStormTracks")]
        public IActionResult GetVerticalProfileTropicalStormTracks()
        {
            return Ok();
        }

        [HttpGet(Name = "GetCrossedWeather")]
        public IActionResult GetCrossedWeather()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWxAreas")]
        public IActionResult GetSigWxAreas()
        {
            return Ok();
        }

        [HttpGet(Name = "GetParsedAirportWeather")]
        public IActionResult GetParsedAirportWeather()
        {
            return Ok();
        }

        [HttpGet(Name = "GetVulcanicAshData")]
        public IActionResult GetVulcanicAshData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetDataSourceById")]
        public IActionResult GetDataSourceById()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveDataSource")]
        public IActionResult SaveDataSource()
        {
            return Ok();
        }

        [HttpGet(Name = "GetDataSources")]
        public IActionResult GetDataSources()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveOpmetData")]
        public IActionResult SaveOpmetData()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveVolcanicAshData")]
        public IActionResult SaveVolcanicAshData()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveTropicalStormTracksData")]
        public IActionResult SaveTropicalStormTracksData()
        {
            return Ok();
        }

        [HttpGet(Name = "SavePirepData")]
        public IActionResult SavePirepData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetPirep")]
        public IActionResult GetPirep()
        {
            return Ok();
        }

        [HttpGet(Name = "GetPirepForRoute")]
        public IActionResult GetPirepForRoute()
        {
            return Ok();
        }

        [HttpGet(Name = "GetPirepForFir")]
        public IActionResult GetPirepForFir()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveOpmet")]
        public IActionResult SaveOpmet()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveVolcanicAsh")]
        public IActionResult SaveVolcanicAsh()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveTropicalStormTracks")]
        public IActionResult SaveTropicalStormTracks()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveIcingForecast")]
        public IActionResult SaveIcingForecast()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveUpperAirWindTemperatures")]
        public IActionResult SaveUpperAirWindTemperatures()
        {
            return Ok();
        }

        [HttpGet(Name = "AirportForecastOnDemand")]
        public IActionResult AirportForecastOnDemand()
        {
            return Ok();
        }

        [HttpGet(Name = "CancelOpmetData")]
        public IActionResult CancelOpmetData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetHistoricalAirportWeather")]
        public IActionResult GetHistoricalAirportWeather()
        {
            return Ok();
        }

        [HttpGet(Name = "GetFlightWeather")]
        public IActionResult GetFlightWeather()
        {
            return Ok();
        }

        [HttpGet(Name = "GetFlightWeatherForecastOnDemand")]
        public IActionResult GetFlightWeatherForecastOnDemand()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastConvection")]
        public IActionResult SaveSigWXPrognosisForecastConvection()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastJetStreamPointData")]
        public IActionResult SaveSigWXPrognosisForecastJetStreamPointData()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastTropopause")]
        public IActionResult SaveSigWXPrognosisForecastTropopause()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastTurbulence")]
        public IActionResult SaveSigWXPrognosisForecastTurbulence()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastTurbulencePointData")]
        public IActionResult SaveSigWXPrognosisForecastTurbulencePointData()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastJetStream")]
        public IActionResult SaveSigWXPrognosisForecastJetStream()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastStorm")]
        public IActionResult SaveSigWXPrognosisForecastStorm()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastVolcano")]
        public IActionResult SaveSigWXPrognosisForecastVolcano()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSigWXPrognosisForecastRadiation")]
        public IActionResult SaveSigWXPrognosisForecastRadiation()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastConvection")]
        public IActionResult GetSigWXPrognosisForecastConvection()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastJetStreamPointData")]
        public IActionResult GetSigWXPrognosisForecastJetStreamPointData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastTropopause")]
        public IActionResult GetSigWXPrognosisForecastTropopause()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastTurbulence")]
        public IActionResult GetSigWXPrognosisForecastTurbulence()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastTurbulencePointData")]
        public IActionResult GetSigWXPrognosisForecastTurbulencePointData()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastJetStream")]
        public IActionResult GetSigWXPrognosisForecastJetStream()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastStorm")]
        public IActionResult GetSigWXPrognosisForecastStorm()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastVolcano")]
        public IActionResult GetSigWXPrognosisForecastVolcano()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSigWXPrognosisForecastRadiation")]
        public IActionResult GetSigWXPrognosisForecastRadiation()
        {
            return Ok();
        }

        [HttpGet(Name = "SaveSurface")]
        public IActionResult SaveSurface()
        {
            return Ok();
        }

        [HttpGet(Name = "GetSurface")]
        public IActionResult GetSurface()
        {
            return Ok();
        }

        [HttpGet(Name = "GetUpperAirWindTemperatures")]
        public IActionResult GetUpperAirWindTemperatures()
        {
            return Ok();
        }

        [HttpGet(Name = "GetPointWeatherUi")]
        public IActionResult GetPointWeatherUi()
        {
            return Ok();
        }

        [HttpGet(Name = "GetAirspaceByUniqueKey")]
        public IActionResult GetAirspaceByUniqueKey()
        {
            return Ok();
        }

    }
}