using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace ORTSMonitor
{
    public class TrainInfo
    {
        public string ControlMode { get; set; }
        // Přidejte další vlastnosti podle potřeby
    }

    public class TrackMonitorDisplayItem
    {
        public string FirstCol { get; set; }
        public string TrackColLeft { get; set; }
        public string TrackCol { get; set; }
        public string TrackColRight { get; set; }
        public string LimitCol { get; set; }
        public string SignalCol { get; set; }
        public string DistCol { get; set; }
        public object TrackColItem { get; set; }
        public object SignalColItem { get; set; }
    }

    public class TrainDrivingDisplayItem
    {
        public string FirstCol { get; set; }
        public string LastCol { get; set; }
        public string KeyPressed { get; set; }
        public string SymbolCol { get; set; }
    }

    public class TrainDrivingData
    {
        public string Time { get; set; }
        public double? Speed { get; set; }
        public double? Limit { get; set; }
        public double? Distance { get; set; }
        public string Direction { get; set; }
        public double? Throttle { get; set; }
        public string TrainBrake { get; set; }
        public double? EQ { get; set; }
        public double? BC { get; set; }
        public double? BP { get; set; }
        public double? EngineBrake { get; set; }
        public double? DynamicBrake { get; set; }
    }

    public class TrackMonitorData
    {
        public string ControlMode { get; set; }
        public List<TrackMonitorRow> Rows { get; set; } = new List<TrackMonitorRow>();
    }

    public class TrackMonitorRow
    {
        public int RowIndex { get; set; }
        public string FirstCol { get; set; }
        public string TrackColLeft { get; set; }
        public string TrackCol { get; set; }
        public string TrackColRight { get; set; }
        public string LimitCol { get; set; }
        public string SignalCol { get; set; }
        public string DistCol { get; set; }
        public bool IsSeparator { get; set; }
        public string SeparatorType { get; set; } // "Sprtr", "SprtrRed", "SprtrDarkGray"
        public bool HasColorCoding { get; set; }
        public double? LimitValue { get; set; } // Číselná hodnota z LimitCol
        public double? DistanceValue { get; set; } // Číselná hodnota z DistCol
    }

    public class CabControlItem
    {
        public string TypeName { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double RangeFraction { get; set; }

        // Computed properties
        public double Range => MaxValue - MinValue;
        public double CurrentValue => RangeFraction * Range + MinValue;
        public double ScalePercentage => RangeFraction * 100;
        public string FormattedCurrentValue
        {
            get
            {
                if (Range == 0)
                    return "-";

                int decimalPlaces = 4;
                if (Range > 0.01) decimalPlaces -= 1;
                if (Range > 0.1) decimalPlaces -= 1;
                if (Range > 2) decimalPlaces -= 1;
                if (Range > 10) decimalPlaces -= 1;

                return CurrentValue.ToString($"F{decimalPlaces}");
            }
        }

        public string FormattedTypeName
        {
            get
            {
                // Replace "_" with " " and capitalize each word
                string typeName = TypeName.Replace("_", " ");
                var words = typeName.ToLower().Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length > 0)
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
                return string.Join(" ", words);
            }
        }
    }

    public class CabControlsData
    {
        public List<CabControlItem> Controls { get; set; } = new List<CabControlItem>();
        public int ControlsCount => Controls.Count;
    }

    public class HudTable
    {
        public int nRows { get; set; }
        public int nCols { get; set; }
        public List<string> values { get; set; } = new List<string>();
    }

    public class HudData
    {
        public int nTables { get; set; }
        public HudTable commonTable { get; set; }
        public HudTable extraTable { get; set; }
    }

    public class HudResponse
    {
        public int PageNumber { get; set; }
        public string PageName { get; set; }
        public HudData Data { get; set; }
        public List<List<string>> CommonTableRows { get; set; } = new List<List<string>>();
        public List<List<string>> ExtraTableRows { get; set; } = new List<List<string>>();

        public static string GetPageName(int pageNo)
        {
            return pageNo switch
            {
                0 => "Common",
                1 => "Consist",
                2 => "Locomotive",
                3 => "Brake",
                4 => "Force",
                5 => "Dispatcher",
                6 => "Weather",
                7 => "Debug",
                _ => "Unknown"
            };
        }
    }

    public class HudForceData
    {
        public Dictionary<string, string> CommonInfo { get; set; } = new Dictionary<string, string>();
        public HudForceDetails ForceDetails { get; set; } = new HudForceDetails();
        public List<HudCarForceData> CarForces { get; set; } = new List<HudCarForceData>();
    }

    public class HudForceDetails
    {
        public double? WheelSlip { get; set; } // Percentage
        public double? Conditions { get; set; } // Percentage
        public double? AxleDriveForce { get; set; } // N or kN
        public double? AxleBrakeForce { get; set; } // N or kN
        public string NumberOfSubsteps { get; set; }
        public string Solver { get; set; }
        public string StabilityCorrection { get; set; }
        public double? AxleOutForce { get; set; } // N or kN
        public double? WheelSpeed { get; set; } // km/h
        public double? LocoAdhesion { get; set; } // Percentage
        public double? WagonAdhesion { get; set; } // Percentage
        public double? TrackQuality { get; set; } // Percentage
        public HudWindData WindData { get; set; } = new HudWindData();
    }

    public class HudWindData
    {
        public string WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public string TrainDirection { get; set; }
        public string ResWind { get; set; }
        public string ResSpeed { get; set; }
    }

    public class HudCarForceData
    {
        public string Car { get; set; }
        public double? Total { get; set; } // N or kN
        public double? Motive { get; set; } // N or kN
        public double? Brake { get; set; } // N or kN
        public double? Friction { get; set; } // N or kN
        public double? Gravity { get; set; } // N or kN
        public double? Curve { get; set; } // N or kN
        public double? Tunnel { get; set; } // N or kN
        public double? Wind { get; set; } // N or kN
        public double? Coupler1 { get; set; } // N or kN
        public double? Coupler2 { get; set; } // N or kN
        public double? Slack { get; set; } // meters
        public double? Mass { get; set; } // tons
        public double? Gradient { get; set; } // percentage
        public double? CurveRadius { get; set; } // meters
        public double? BrakeFriction { get; set; } // percentage
        public string BrakeSlide { get; set; }
        public double? BearingTemp { get; set; } // degrees Celsius
    }

    public class OpenRailsWebDataReader
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string[] _colorCodes = { "???", "??!", "?!?", "?!!", "!??", "!!?", "!!!", "%%%", "%$$", "%%$", "$%$", "$$$" };

        public string BaseURL {  get {  return _baseUrl; } }

        public OpenRailsWebDataReader(string baseUrl = "http://localhost:2150")
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
        }

        public async Task<T> ApiGetAsync<T>(string endpoint)
        {
            try
            {
                string url = $"{_baseUrl}/API/{endpoint}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    throw new HttpRequestException($"API call failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Chyba při volání API: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetTrackMonitorDataAsync()
        {
            try
            {
                // Paralelní načtení dat ze dvou endpointů
                Task<TrainInfo> trainInfoTask = ApiGetAsync<TrainInfo>("TRAININFO");
                Task<List<TrackMonitorDisplayItem>> trackMonitorTask = ApiGetAsync<List<TrackMonitorDisplayItem>>("TRACKMONITORDISPLAY");

                await Task.WhenAll(trainInfoTask, trackMonitorTask);

                TrainInfo trainInfo = trainInfoTask.Result;
                List<TrackMonitorDisplayItem> trackMonitorDisplay = trackMonitorTask.Result;

                StringBuilder output = new StringBuilder();
                output.AppendLine("=== TRACK MONITOR ===");
                output.AppendLine($"Control Mode: {trainInfo.ControlMode}");
                output.AppendLine();

                // Hlavička tabulky
                output.AppendLine(string.Format("{0,-15} {1,-10} {2,-15} {3,-10} {4,-10} {5,-10} {6,-10}",
                    "First", "TrkLeft", "Track", "TrkRight", "Limit", "Signal", "Distance"));
                output.AppendLine(new string('-', 85));

                foreach (var item in trackMonitorDisplay)
                {
                    if (item.FirstCol == "Sprtr")
                    {
                        output.AppendLine(new string('-', 85));
                        continue;
                    }
                    if (item.FirstCol == "SprtrRed")
                    {
                        output.AppendLine(new string('=', 85));
                        continue;
                    }
                    if (item.FirstCol == "SprtrDarkGray")
                    {
                        output.AppendLine(new string('.', 85));
                        continue;
                    }

                    string firstCol = ProcessColoredText(item.FirstCol);
                    string trackColLeft = ProcessColoredText(item.TrackColLeft);
                    string trackCol = ProcessColoredText(item.TrackCol);
                    string trackColRight = ProcessColoredText(item.TrackColRight);
                    string limitCol = ProcessColoredText(item.LimitCol);
                    string signalCol = ProcessColoredText(item.SignalCol);
                    string distCol = ProcessColoredText(item.DistCol);

                    output.AppendLine(string.Format("{0,-15} {1,-10} {2,-15} {3,-10} {4,-10} {5,-10} {6,-10}",
                        firstCol ?? "", trackColLeft ?? "", trackCol ?? "", trackColRight ?? "",
                        limitCol ?? "", signalCol ?? "", distCol ?? ""));
                }

                return output.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: Chyba při načítání Track Monitor dat: {ex.Message}";
            }
        }

        public async Task<string> GetTrainDrivingDataAsync(bool normalTextMode = true)
        {
            try
            {
                List<TrainDrivingDisplayItem> trainDrivingData =
                    await ApiGetAsync<List<TrainDrivingDisplayItem>>($"TRAINDRIVINGDISPLAY?normalText={normalTextMode}");

                StringBuilder output = new StringBuilder();
                output.AppendLine("=== TRAIN DRIVING INFO ===");
                output.AppendLine();

                // Hlavička tabulky
                output.AppendLine(string.Format("{0,-5} {1,-30} {2,-5} {3,-30}",
                    "Key", "First Column", "Sym", "Last Column"));
                output.AppendLine(new string('-', 72));

                foreach (var item in trainDrivingData)
                {
                    if (item.FirstCol == "Sprtr")
                    {
                        output.AppendLine(new string('-', 72));
                        continue;
                    }

                    string firstCol = ProcessColoredText(item.FirstCol);
                    string lastCol = ProcessColoredText(item.LastCol);
                    string keyPressed = ProcessColoredText(item.KeyPressed);
                    string symbolCol = ProcessColoredText(item.SymbolCol);

                    output.AppendLine(string.Format("{0,-5} {1,-30} {2,-5} {3,-30}",
                        keyPressed ?? "", firstCol ?? "", symbolCol ?? "", lastCol ?? ""));
                }

                return output.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: Chyba při načítání Train Driving dat: {ex.Message}";
            }
        }

        public async Task<CabControlsData> ParseCabControlsDataAsync()
        {
            try
            {
                List<CabControlItem> cabControls = await ApiGetAsync<List<CabControlItem>>("CABCONTROLS");

                return new CabControlsData
                {
                    Controls = cabControls ?? new List<CabControlItem>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Chyba při načítání Cab Controls dat: {ex.Message}");
                return new CabControlsData();
            }
        }

        public async Task<string> GetCabControlsDataAsync()
        {
            try
            {
                CabControlsData cabData = await ParseCabControlsDataAsync();

                StringBuilder output = new StringBuilder();
                output.AppendLine("=== CAB CONTROLS ===");
                output.AppendLine();

                // Hlavička tabulky
                output.AppendLine(string.Format("{0,-25} {1,-8} {2,-12} {3,-8} {4,-8}",
                    "Control", "Min", "Value", "Max", "Scale%"));
                output.AppendLine(new string('-', 65));

                foreach (var control in cabData.Controls)
                {
                    output.AppendLine(string.Format("{0,-25} {1,-8:F2} {2,-12} {3,-8:F2} {4,-8:F1}%",
                        control.FormattedTypeName,
                        control.MinValue,
                        control.FormattedCurrentValue,
                        control.MaxValue,
                        control.ScalePercentage));
                }

                output.AppendLine();
                output.AppendLine($"Celkem ovládacích prvků: {cabData.ControlsCount}");

                return output.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: Chyba při načítání Cab Controls dat: {ex.Message}";
            }
        }

        public CabControlsData ParseCabControlsText(string text)
        {
            var data = new CabControlsData();
            string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string cleanLine = line.Trim();

                // Přeskočíme hlavičky a oddělovače
                if (cleanLine.StartsWith("===") || cleanLine.StartsWith("Control") ||
                    cleanLine.StartsWith("---") || cleanLine.Length < 10)
                    continue;

                // Parsování řádku dat (očekáváme formát: Control Min Value Max Scale%)
                var parts = cleanLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    try
                    {
                        var control = new CabControlItem();

                        // První část je název (může obsahovat více slov)
                        int controlNameEndIndex = 0;
                        for (int i = parts.Length - 4; i >= 0; i--)
                        {
                            if (double.TryParse(parts[i], out _))
                            {
                                controlNameEndIndex = i - 1;
                                break;
                            }
                        }

                        if (controlNameEndIndex >= 0)
                        {
                            control.TypeName = string.Join("_", parts.Take(controlNameEndIndex + 1));

                            // Parsování číselných hodnot (posledních 3-4 částí)
                            var numericParts = parts.Skip(controlNameEndIndex + 1).ToArray();
                            if (numericParts.Length >= 3)
                            {
                                if (double.TryParse(numericParts[0], out double min))
                                    control.MinValue = min;

                                string valueStr = numericParts[1];
                                if (valueStr != "-" && double.TryParse(valueStr, out double value))
                                {
                                    if (double.TryParse(numericParts[2], out double max))
                                    {
                                        control.MaxValue = max;
                                        double range = max - min;
                                        if (range > 0)
                                            control.RangeFraction = (value - min) / range;
                                        else
                                            control.RangeFraction = 0;
                                    }
                                }

                                data.Controls.Add(control);
                            }
                        }
                    }
                    catch
                    {
                        // Ignorujeme řádky, které se nepodařilo parsovat
                        continue;
                    }
                }
            }

            return data;
        }

        public async Task<HudResponse> GetHudDataAsync(int pageNo = 0)
        {
            try
            {
                HudData hudData = await ApiGetAsync<HudData>($"HUD/{pageNo}");

                var response = new HudResponse
                {
                    PageNumber = pageNo,
                    PageName = HudResponse.GetPageName(pageNo),
                    Data = hudData
                };

                // Parse common table into rows
                if (hudData.commonTable != null && hudData.commonTable.values != null)
                {
                    response.CommonTableRows = ParseHudTableToRows(hudData.commonTable);
                }

                // Parse extra table into rows if present
                if (hudData.nTables == 2 && hudData.extraTable != null && hudData.extraTable.values != null)
                {
                    response.ExtraTableRows = ParseHudTableToRows(hudData.extraTable);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Chyba při načítání HUD dat: {ex.Message}");
                return new HudResponse
                {
                    PageNumber = pageNo,
                    PageName = HudResponse.GetPageName(pageNo),
                    Data = new HudData()
                };
            }
        }

        public async Task<string> GetHudDataAsStringAsync(int pageNo = 0)
        {
            try
            {
                HudResponse hudResponse = await GetHudDataAsync(pageNo);

                StringBuilder output = new StringBuilder();
                output.AppendLine($"=== HUD - {hudResponse.PageName.ToUpper()} ===");
                output.AppendLine();

                // Common table
                if (hudResponse.CommonTableRows.Any())
                {
                    output.AppendLine("Common Information:");
                    output.AppendLine(new string('-', 50));

                    foreach (var row in hudResponse.CommonTableRows)
                    {
                        var cleanedRow = row.Select(cell => ProcessColoredText(cell) ?? "").ToList();
                        output.AppendLine(string.Join(" | ", cleanedRow).Trim());
                    }
                }

                // Extra table
                if (hudResponse.ExtraTableRows.Any())
                {
                    output.AppendLine();
                    output.AppendLine($"{hudResponse.PageName} Detailed Information:");
                    output.AppendLine(new string('-', 50));

                    foreach (var row in hudResponse.ExtraTableRows)
                    {
                        var cleanedRow = row.Select(cell => ProcessColoredText(cell) ?? "").ToList();
                        string rowText = string.Join(" | ", cleanedRow).Trim();

                        // Special formatting for information headers
                        if (rowText.Contains("INFORMATION"))
                        {
                            output.AppendLine();
                            output.AppendLine($"=== {rowText} ===");
                        }
                        else if (!string.IsNullOrWhiteSpace(rowText))
                        {
                            output.AppendLine(rowText);
                        }
                    }
                }

                return output.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: Chyba při načítání HUD dat: {ex.Message}";
            }
        }

        public async Task<List<HudResponse>> GetAllHudPagesAsync()
        {
            var allPages = new List<HudResponse>();

            for (int pageNo = 0; pageNo <= 7; pageNo++)
            {
                try
                {
                    var page = await GetHudDataAsync(pageNo);
                    allPages.Add(page);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading HUD page {pageNo}: {ex.Message}");
                    allPages.Add(new HudResponse
                    {
                        PageNumber = pageNo,
                        PageName = HudResponse.GetPageName(pageNo),
                        Data = new HudData()
                    });
                }
            }

            return allPages;
        }

        private List<List<string>> ParseHudTableToRows(HudTable table)
        {
            var rows = new List<List<string>>();

            if (table.values == null || table.values.Count == 0)
                return rows;

            int valueIndex = 0;
            for (int row = 0; row < table.nRows; row++)
            {
                var rowData = new List<string>();
                for (int col = 0; col < table.nCols; col++)
                {
                    if (valueIndex < table.values.Count)
                    {
                        rowData.Add(table.values[valueIndex] ?? "");
                        valueIndex++;
                    }
                    else
                    {
                        rowData.Add("");
                    }
                }
                rows.Add(rowData);
            }

            return rows;
        }

        public HudForceData ParseHudForceText(string text)
        {
            var data = new HudForceData();
            string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            bool inCommonInfo = false;
            bool inForceDetails = false;
            bool inCarTable = false;

            foreach (string line in lines)
            {
                string cleanLine = line.Trim();

                // Check for section headers
                if (cleanLine.Contains("Common Information"))
                {
                    inCommonInfo = true;
                    inForceDetails = false;
                    inCarTable = false;
                    continue;
                }
                else if (cleanLine.Contains("Force Detailed Information"))
                {
                    inCommonInfo = false;
                    inForceDetails = true;
                    inCarTable = false;
                    continue;
                }

                // Skip headers and separators
                if (cleanLine.StartsWith("===") || cleanLine.All(c => c == '-') || string.IsNullOrWhiteSpace(cleanLine))
                {
                    continue;
                }

                // Parse common information
                if (inCommonInfo && cleanLine.Contains("|"))
                {
                    ParseCommonInfoLine(cleanLine, data);
                    continue;
                }

                // Parse force details
                if (inForceDetails)
                {
                    if (cleanLine.Contains("Wind Speed:"))
                    {
                        ParseWindLine(cleanLine, data.ForceDetails.WindData);
                        continue;
                    }
                    else if (cleanLine.StartsWith("Car |") || cleanLine.Contains("Car | Total"))
                    {
                        inCarTable = true;
                        continue;
                    }
                    else if (inCarTable && (cleanLine.Contains(" - ") || cleanLine.StartsWith("0 - ")))
                    {
                        ParseCarForceLine(cleanLine, data);
                        continue;
                    }
                    else if (cleanLine.Contains("|") && !cleanLine.StartsWith("|") && !inCarTable)
                    {
                        ParseForceDetailLine(cleanLine, data.ForceDetails);
                    }
                }
            }

            return data;
        }

        private void ParseCommonInfoLine(string line, HudForceData data)
        {
            // Pattern: Key |  | Value  or  Key | | Value
            var parts = line.Split('|');
            if (parts.Length >= 3)
            {
                string key = parts[0].Trim();
                string value = parts[2].Trim(); // Value is always in the third part (index 2)

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    data.CommonInfo[key] = value;
                }
            }
        }

        private void ParseForceDetailLine(string line, HudForceDetails details)
        {
            var parts = line.Split('|');
            if (parts.Length >= 3)
            {
                string key = parts[0].Trim();
                string value = "";

                // Find the first non-empty value after the key
                for (int i = 2; i < parts.Length; i++)
                {
                    string part = parts[i].Trim();
                    if (!string.IsNullOrEmpty(part))
                    {
                        value = part;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                    return;

                switch (key.ToLower())
                {
                    case "wheel slip":
                        details.WheelSlip = ParsePercentageValue(value);
                        break;
                    case "conditions":
                        details.Conditions = ParsePercentageValue(value);
                        break;
                    case "axle drive force":
                        details.AxleDriveForce = ParseForceValue(value);
                        break;
                    case "axle brake force":
                        details.AxleBrakeForce = ParseForceValue(value);
                        break;
                    case "number of substeps":
                        details.NumberOfSubsteps = value;
                        break;
                    case "solver":
                        details.Solver = value;
                        break;
                    case "stability correction":
                        details.StabilityCorrection = value;
                        break;
                    case "axle out force":
                        details.AxleOutForce = ParseForceValue(value);
                        break;
                    case "wheel speed":
                        details.WheelSpeed = ParseSpeedValue(value);
                        break;
                    case "loco adhesion":
                        details.LocoAdhesion = ParsePercentageValue(value);
                        break;
                    case "wagon adhesion":
                        details.WagonAdhesion = ParsePercentageValue(value);
                        break;
                    case "track quality":
                        details.TrackQuality = ParsePercentageValue(value);
                        break;
                }
            }
        }

        private void ParseWindLine(string line, HudWindData windData)
        {
            // Wind Speed: | 8,50 mph | Wind Direction: |  | 262,57 Deg | Train Direction: |  | 167,64 Deg | ResWind: | 94,93 mph | ResSpeed: | 8,50 mph |
            var parts = line.Split('|');

            for (int i = 0; i < parts.Length - 1; i++)
            {
                string key = parts[i].Trim();
                string value = i + 1 < parts.Length ? parts[i + 1].Trim() : "";

                if (key.Contains("Wind Speed:") && !string.IsNullOrEmpty(value))
                    windData.WindSpeed = value;
                else if (key.Contains("Wind Direction:") && !string.IsNullOrEmpty(value))
                    windData.WindDirection = value;
                else if (key.Contains("Train Direction:") && !string.IsNullOrEmpty(value))
                    windData.TrainDirection = value;
                else if (key.Contains("ResWind:") && !string.IsNullOrEmpty(value))
                    windData.ResWind = value;
                else if (key.Contains("ResSpeed:") && !string.IsNullOrEmpty(value))
                    windData.ResSpeed = value;
            }
        }

        private void ParseCarForceLine(string line, HudForceData data)
        {
            var parts = line.Split('|');
            if (parts.Length >= 17)
            {
                var car = new HudCarForceData
                {
                    Car = parts[0].Trim(),
                    Total = ParseForceValue(parts[1].Trim()),
                    Motive = ParseForceValue(parts[2].Trim()),
                    Brake = ParseForceValue(parts[3].Trim()),
                    Friction = ParseForceValue(parts[4].Trim()),
                    Gravity = ParseForceValue(parts[5].Trim()),
                    Curve = ParseForceValue(parts[6].Trim()),
                    Tunnel = ParseForceValue(parts[7].Trim()),
                    Wind = ParseForceValue(parts[8].Trim()),
                    Coupler1 = ParseForceValue(parts[9].Trim()),
                    Coupler2 = ParseForceValue(parts[10].Trim()),
                    Slack = ParseDistanceValue(parts[11].Trim()),
                    Mass = ParseMassValue(parts[12].Trim()),
                    Gradient = ParsePercentageValue(parts[13].Trim()),
                    CurveRadius = ParseDistanceValue(parts[14].Trim()),
                    BrakeFriction = ParsePercentageValue(parts[15].Trim()),
                    BrakeSlide = CleanValue(parts[16].Trim())
                };

                if (parts.Length >= 18)
                    car.BearingTemp = ParseTemperatureValue(parts[17].Trim());

                data.CarForces.Add(car);
            }
        }

        public string FormatHudForceData(HudForceData data)
        {
            var output = new StringBuilder();
            output.AppendLine("=== HUD FORCE DATA ===");
            output.AppendLine();

            // Common Information
            output.AppendLine("Common Information:");
            output.AppendLine(new string('-', 30));
            foreach (var kvp in data.CommonInfo)
            {
                output.AppendLine($"{kvp.Key}: {kvp.Value}");
            }

            output.AppendLine();
            output.AppendLine("Force Details:");
            output.AppendLine(new string('-', 30));

            if (data.ForceDetails.WheelSlip.HasValue)
                output.AppendLine($"Wheel Slip: {data.ForceDetails.WheelSlip:F1}%");
            if (data.ForceDetails.Conditions.HasValue)
                output.AppendLine($"Conditions: {data.ForceDetails.Conditions:F1}%");
            if (data.ForceDetails.AxleDriveForce.HasValue)
                output.AppendLine($"Axle Drive Force: {data.ForceDetails.AxleDriveForce:F0} N");
            if (data.ForceDetails.AxleBrakeForce.HasValue)
                output.AppendLine($"Axle Brake Force: {data.ForceDetails.AxleBrakeForce:F0} N");
            if (data.ForceDetails.AxleOutForce.HasValue)
                output.AppendLine($"Axle Out Force: {data.ForceDetails.AxleOutForce:F0} N");
            if (data.ForceDetails.WheelSpeed.HasValue)
                output.AppendLine($"Wheel Speed: {data.ForceDetails.WheelSpeed:F1} km/h");
            if (data.ForceDetails.LocoAdhesion.HasValue)
                output.AppendLine($"Loco Adhesion: {data.ForceDetails.LocoAdhesion:F1}%");
            if (data.ForceDetails.WagonAdhesion.HasValue)
                output.AppendLine($"Wagon Adhesion: {data.ForceDetails.WagonAdhesion:F1}%");
            if (data.ForceDetails.TrackQuality.HasValue)
                output.AppendLine($"Track Quality: {data.ForceDetails.TrackQuality:F1}%");

            // Wind Data
            if (!string.IsNullOrEmpty(data.ForceDetails.WindData.WindSpeed))
            {
                output.AppendLine();
                output.AppendLine("Wind Information:");
                output.AppendLine($"  Wind Speed: {data.ForceDetails.WindData.WindSpeed}");
                output.AppendLine($"  Wind Direction: {data.ForceDetails.WindData.WindDirection}");
                output.AppendLine($"  Train Direction: {data.ForceDetails.WindData.TrainDirection}");
                output.AppendLine($"  Resultant Wind: {data.ForceDetails.WindData.ResWind}");
                output.AppendLine($"  Resultant Speed: {data.ForceDetails.WindData.ResSpeed}");
            }

            // Car Forces
            if (data.CarForces.Any())
            {
                output.AppendLine();
                output.AppendLine($"Car Forces ({data.CarForces.Count} cars):");
                output.AppendLine(new string('-', 50));

                foreach (var car in data.CarForces.Take(5)) // Show first 5 cars
                {
                    string totalStr = car.Total.HasValue ? $"{car.Total:F0} N" : "N/A";
                    string brakeStr = car.Brake.HasValue ? $"{car.Brake:F0} N" : "N/A";
                    string massStr = car.Mass.HasValue ? $"{car.Mass:F1} t" : "N/A";
                    output.AppendLine($"Car {car.Car}: Total={totalStr}, Brake={brakeStr}, Mass={massStr}");
                }

                if (data.CarForces.Count > 5)
                    output.AppendLine($"... and {data.CarForces.Count - 5} more cars");
            }

            return output.ToString();
        }

        public async Task<TrackMonitorData> ParseTrackMonitorDataAsync()
        {
            try
            {
                // Paralelní načtení dat ze dvou endpointů
                Task<TrainInfo> trainInfoTask = ApiGetAsync<TrainInfo>("TRAININFO");
                Task<List<TrackMonitorDisplayItem>> trackMonitorTask = ApiGetAsync<List<TrackMonitorDisplayItem>>("TRACKMONITORDISPLAY");

                await Task.WhenAll(trainInfoTask, trackMonitorTask);

                TrainInfo trainInfo = trainInfoTask.Result;
                List<TrackMonitorDisplayItem> trackMonitorDisplay = trackMonitorTask.Result;

                return ParseTrackMonitorInfo(trainInfo, trackMonitorDisplay);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Chyba při načítání Track Monitor dat: {ex.Message}");
                return new TrackMonitorData();
            }
        }

        public TrackMonitorData ParseTrackMonitorText(string text)
        {
            var data = new TrackMonitorData();
            string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            int rowIndex = 0;
            foreach (string line in lines)
            {
                string cleanLine = line.Trim();

                // Přeskočíme hlavičky
                if (cleanLine.StartsWith("===") || cleanLine.StartsWith("First") ||
                    cleanLine.Contains("Control Mode:") || cleanLine.Length < 5)
                {
                    if (cleanLine.Contains("Control Mode:"))
                    {
                        var parts = cleanLine.Split(':');
                        if (parts.Length > 1)
                            data.ControlMode = parts[1].Trim();
                    }
                    continue;
                }

                // Detekce oddělovačů
                if (cleanLine.All(c => c == '-' || c == '=' || c == '.'))
                {
                    var separatorRow = new TrackMonitorRow
                    {
                        RowIndex = rowIndex++,
                        IsSeparator = true,
                        SeparatorType = cleanLine.Contains('=') ? "SprtrRed" :
                                      cleanLine.Contains('.') ? "SprtrDarkGray" : "Sprtr"
                    };
                    data.Rows.Add(separatorRow);
                    continue;
                }

                // Parsování řádku dat
                if (cleanLine.Length > 70) // Dostatečně dlouhý řádek s daty
                {
                    var row = ParseTrackMonitorRow(cleanLine, rowIndex++);
                    if (row != null)
                        data.Rows.Add(row);
                }
            }

            return data;
        }

        private TrackMonitorData ParseTrackMonitorInfo(TrainInfo trainInfo, List<TrackMonitorDisplayItem> trackMonitorDisplay)
        {
            var data = new TrackMonitorData
            {
                ControlMode = trainInfo.ControlMode
            };

            for (int i = 0; i < trackMonitorDisplay.Count; i++)
            {
                var item = trackMonitorDisplay[i];
                var row = new TrackMonitorRow
                {
                    RowIndex = i,
                    FirstCol = ProcessColoredText(item.FirstCol),
                    TrackColLeft = ProcessColoredText(item.TrackColLeft),
                    TrackCol = ProcessColoredText(item.TrackCol),
                    TrackColRight = ProcessColoredText(item.TrackColRight),
                    LimitCol = ProcessColoredText(item.LimitCol),
                    SignalCol = ProcessColoredText(item.SignalCol),
                    DistCol = ProcessColoredText(item.DistCol)
                };

                // Detekce separátorů
                if (item.FirstCol == "Sprtr" || item.FirstCol == "SprtrRed" || item.FirstCol == "SprtrDarkGray")
                {
                    row.IsSeparator = true;
                    row.SeparatorType = item.FirstCol;
                }

                // Parsování číselných hodnot
                row.LimitValue = ParseDoubleValue(row.LimitCol);
                row.DistanceValue = ParseDoubleValue(row.DistCol);

                // Detekce barevného kódování
                row.HasColorCoding = HasColorCode(item.FirstCol) || HasColorCode(item.TrackColLeft) ||
                                   HasColorCode(item.TrackCol) || HasColorCode(item.TrackColRight) ||
                                   HasColorCode(item.LimitCol) || HasColorCode(item.SignalCol) ||
                                   HasColorCode(item.DistCol);

                data.Rows.Add(row);
            }

            return data;
        }

        private TrackMonitorRow ParseTrackMonitorRow(string line, int rowIndex)
        {
            try
            {
                // Rozdělení řádku podle pozic sloupců (přibližně podle formátu)
                var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3)
                    return null;

                var row = new TrackMonitorRow
                {
                    RowIndex = rowIndex,
                    FirstCol = parts.Length > 0 ? CleanValue(parts[0]) : null,
                    TrackColLeft = parts.Length > 1 ? CleanValue(parts[1]) : null,
                    TrackCol = parts.Length > 2 ? CleanValue(parts[2]) : null,
                    TrackColRight = parts.Length > 3 ? CleanValue(parts[3]) : null,
                    LimitCol = parts.Length > 4 ? CleanValue(parts[4]) : null,
                    SignalCol = parts.Length > 5 ? CleanValue(parts[5]) : null,
                    DistCol = parts.Length > 6 ? CleanValue(parts[6]) : null
                };

                // Parsování číselných hodnot
                row.LimitValue = ParseDoubleValue(row.LimitCol);
                row.DistanceValue = ParseDoubleValue(row.DistCol);

                return row;
            }
            catch
            {
                return null;
            }
        }

        private bool HasColorCode(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < 3)
                return false;

            string colorCode = text.Substring(text.Length - 3);
            return Array.IndexOf(_colorCodes, colorCode) != -1;
        }

        public async Task<TrainDrivingData> ParseTrainDrivingDataAsync(bool normalTextMode = true)
        {
            try
            {
                List<TrainDrivingDisplayItem> trainDrivingData =
                    await ApiGetAsync<List<TrainDrivingDisplayItem>>($"TRAINDRIVINGDISPLAY?normalText={normalTextMode}");

                return ParseTrainDrivingInfo(trainDrivingData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Chyba při načítání Train Driving dat: {ex.Message}");
                return new TrainDrivingData();
            }
        }

        public TrainDrivingData ParseTrainDrivingText(string text)
        {
            var data = new TrainDrivingData();
            string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string cleanLine = line.Trim();

                // Přeskočíme hlavičky a oddělovače
                if (cleanLine.StartsWith("===") || cleanLine.StartsWith("Key") ||
                    cleanLine.StartsWith("---") || cleanLine.Length < 5)
                    continue;

                // Rozdělíme řádek na sloupce podle pozice
                if (cleanLine.Length > 35)
                {
                    string firstColumn = cleanLine.Substring(6, 30).Trim();
                    string lastColumn = cleanLine.Substring(42).Trim();

                    // Parsujeme jednotlivé hodnoty
                    if (firstColumn.Equals("Time", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Time = CleanValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Speed", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Speed = ParseDoubleValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Limit", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Limit = ParseDoubleValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Distance", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Distance = ParseDoubleValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Direction", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Direction = CleanValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Throttle", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Throttle = ParseDoubleValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Train brake", StringComparison.OrdinalIgnoreCase))
                    {
                        data.TrainBrake = CleanValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Engine brake", StringComparison.OrdinalIgnoreCase))
                    {
                        data.EngineBrake = ParseDoubleValue(lastColumn);
                    }
                    else if (firstColumn.Equals("Dynamic brake", StringComparison.OrdinalIgnoreCase))
                    {
                        data.DynamicBrake = ParseDoubleValue(lastColumn);
                    }
                    // Speciální případy pro EQ, BC, BP
                    else if (lastColumn.Contains("EQ") && lastColumn.Contains("bar"))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(lastColumn, @"EQ\s+(\d+,\d+|\d+\.\d+|\d+)\s+bar");
                        if (match.Success)
                            data.EQ = ParseDoubleFromString(match.Groups[1].Value);
                    }
                    else if (lastColumn.Contains("BC") && lastColumn.Contains("BP") && lastColumn.Contains("bar"))
                    {
                        var bcMatch = System.Text.RegularExpressions.Regex.Match(lastColumn, @"BC\s+(\d+,\d+|\d+\.\d+|\d+)\s+bar");
                        var bpMatch = System.Text.RegularExpressions.Regex.Match(lastColumn, @"BP\s+(\d+,\d+|\d+\.\d+|\d+)\s+bar");

                        if (bcMatch.Success)
                            data.BC = ParseDoubleFromString(bcMatch.Groups[1].Value);
                        if (bpMatch.Success)
                            data.BP = ParseDoubleFromString(bpMatch.Groups[1].Value);
                    }
                }
            }

            return data;
        }

        private TrainDrivingData ParseTrainDrivingInfo(List<TrainDrivingDisplayItem> trainDrivingData)
        {
            var data = new TrainDrivingData();

            foreach (var item in trainDrivingData)
            {
                if (item.FirstCol == "Sprtr" || item.FirstCol == null)
                    continue;

                string firstCol = ProcessColoredText(item.FirstCol) ?? "";
                string lastCol = ProcessColoredText(item.LastCol) ?? "";

                if (firstCol.Equals("Time", StringComparison.OrdinalIgnoreCase))
                {
                    data.Time = CleanValue(lastCol);
                }
                else if (firstCol.Equals("Speed", StringComparison.OrdinalIgnoreCase))
                {
                    data.Speed = ParseDoubleValue(lastCol);
                }
                else if (firstCol.Equals("Limit", StringComparison.OrdinalIgnoreCase))
                {
                    data.Limit = ParseDoubleValue(lastCol);
                }
                else if (firstCol.Equals("Distance", StringComparison.OrdinalIgnoreCase))
                {
                    data.Distance = ParseDoubleValue(lastCol);
                }
                else if (firstCol.Equals("Direction", StringComparison.OrdinalIgnoreCase))
                {
                    data.Direction = CleanValue(lastCol);
                }
                else if (firstCol.Equals("Throttle", StringComparison.OrdinalIgnoreCase))
                {
                    data.Throttle = ParseDoubleValue(lastCol);
                }
                else if (firstCol.Equals("Train brake", StringComparison.OrdinalIgnoreCase))
                {
                    data.TrainBrake = CleanValue(lastCol);
                }
                else if (firstCol.Equals("Engine brake", StringComparison.OrdinalIgnoreCase))
                {
                    data.EngineBrake = ParseDoubleValue(lastCol);
                }
                else if (firstCol.Equals("Dynamic brake", StringComparison.OrdinalIgnoreCase))
                {
                    data.DynamicBrake = ParseDoubleValue(lastCol);
                }
                // Speciální případy pro EQ, BC, BP
                else if (lastCol.Contains("EQ") && lastCol.Contains("bar"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(lastCol, @"EQ\s+(\d+,\d+|\d+\.\d+|\d+)\s+bar");
                    if (match.Success)
                        data.EQ = ParseDoubleFromString(match.Groups[1].Value);
                }
                else if (lastCol.Contains("BC") && lastCol.Contains("BP") && lastCol.Contains("bar"))
                {
                    var bcMatch = System.Text.RegularExpressions.Regex.Match(lastCol, @"BC\s+(\d+,\d+|\d+\.\d+|\d+)\s+bar");
                    var bpMatch = System.Text.RegularExpressions.Regex.Match(lastCol, @"BP\s+(\d+,\d+|\d+\.\d+|\d+)\s+bar");

                    if (bcMatch.Success)
                        data.BC = ParseDoubleFromString(bcMatch.Groups[1].Value);
                    if (bpMatch.Success)
                        data.BP = ParseDoubleFromString(bpMatch.Groups[1].Value);
                }
            }

            return data;
        }

        private string CleanValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Odebereme barevné informace v hranatých závorkách
            int bracketIndex = value.IndexOf(" [");
            if (bracketIndex > 0)
                value = value.Substring(0, bracketIndex);

            return value.Trim();
        }

        private double? ParseDoubleValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Nejprve vyčistíme hodnotu
            string cleanedValue = CleanValue(value);
            if (string.IsNullOrEmpty(cleanedValue))
                return null;

            return ParseDoubleFromString(cleanedValue);
        }

        private double? ParseDoubleFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Nejprve odebereme mezery z čísel (např. "1 085" → "1085")
            string cleanedValue = System.Text.RegularExpressions.Regex.Replace(value, @"(\d)\s+(\d)", "$1$2");

            // Odebereme všechny jednotky a další nečíselné znaky kromě číslic, čárek, teček a záporného znaménka
            string numericPart = System.Text.RegularExpressions.Regex.Match(cleanedValue, @"[-]?(\d+[,.]?\d*|\d*[,.]?\d+)").Value;

            if (string.IsNullOrEmpty(numericPart))
                return null;

            // Nahradíme čárku tečkou pro správné parsování
            numericPart = numericPart.Replace(',', '.');

            if (double.TryParse(numericPart, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return null;
        }

        private double? ParsePercentageValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Odebereme % znak a parsujeme číslo
            string cleanedValue = value.Replace("%", "").Trim();
            return ParseDoubleFromString(cleanedValue);
        }

        private double? ParseForceValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            double? result = ParseDoubleFromString(value);
            if (result.HasValue && value.ToUpper().Contains("KN"))
            {
                // Převedeme kN na N
                result *= 1000;
            }
            return result;
        }

        private double? ParseSpeedValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Extrahujeme první číslo ze stringu (např. "59,7 km/h (0,0 km/h)" -> 59.7)
            var match = System.Text.RegularExpressions.Regex.Match(value, @"^([0-9]+[,.]?[0-9]*)\s*km/h");
            if (match.Success)
            {
                return ParseDoubleFromString(match.Groups[1].Value);
            }

            return ParseDoubleFromString(value);
        }

        private double? ParseDistanceValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Parsování vzdálenosti v metrech (např. "1,5 m" nebo "0,000 m")
            double? result = ParseDoubleFromString(value);
            if (result.HasValue && value.ToUpper().Contains("KM"))
            {
                // Převedeme km na m
                result *= 1000;
            }
            return result;
        }

        private double? ParseMassValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Parsování hmotnosti v tunách (např. "75,4 t")
            double? result = ParseDoubleFromString(value);
            if (result.HasValue && value.ToUpper().Contains("KG"))
            {
                // Převedeme kg na tuny
                result /= 1000;
            }
            return result;
        }

        private double? ParseTemperatureValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Parsování teploty ve stupních Celsia (např. "28,8°C Cool [Orange]" -> 28.8)
            var match = System.Text.RegularExpressions.Regex.Match(value, @"^([0-9]+[,.]?[0-9]*)°?C");
            if (match.Success)
            {
                return ParseDoubleFromString(match.Groups[1].Value);
            }

            return ParseDoubleFromString(value);
        }

        private string ProcessColoredText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            // Pokud text má barevný kód na konci (posledních 3 znaků)
            if (text.Length > 3)
            {
                string colorCode = text.Substring(text.Length - 3);
                if (Array.IndexOf(_colorCodes, colorCode) != -1)
                {
                    // Vrátíme text bez barevného kódu a přidáme informaci o barvě
                    string textWithoutColor = text.Substring(0, text.Length - 3);
                    return $"{textWithoutColor} [{GetColorName(colorCode)}]";
                }
            }

            return text;
        }

        private string GetColorName(string colorCode)
        {
            // Mapování barevných kódů na názvy
            return colorCode switch
            {
                "???" => "Default",
                "??!" => "Yellow",
                "?!?" => "Green",
                "?!!" => "Red",
                "!??" => "Blue",
                "!!?" => "Purple",
                "!!!" => "White",
                "%%%" => "Orange",
                "%$$" => "Brown",
                "%%$" => "Pink",
                "$%$" => "Gray",
                "$$$" => "Black",
                _ => "Unknown"
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

}
