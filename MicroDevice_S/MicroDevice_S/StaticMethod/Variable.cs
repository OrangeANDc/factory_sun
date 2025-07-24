using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroDevice_S.StaticMethod
{
    public static class Variable
    {
        public static Dictionary<string, ushort> _safetyDic = new Dictionary<string, ushort>
        {
            {"Australia---AU AS/NZS 4777.2:2015", 1  },
            {"New Zealand---NZ AS/NZS 4777.2:2015", 2  },
            {"China---CN NB/T 32004:2018", 3  },
            {"Europe/Global---EN 50549-1:2019", 4  },
            {"Netherland---NL EN 50549-1:2019", 5  },
            {"United Kingdom---UK G98-1", 6  },
            {"United Kingdom---UK G99-1", 7  },
            {"Germany---DE VDE-AR-N 4105:2018", 8  },
            {"Brazil---BR ABNT NBR 16149:2013", 9  },
            {"MMexico---Mexico", 10 },

            {"Global---IEC 61727", 13 },
            {"Poland---PL EN 50549-1:2019", 14 },
            {"Vietnam---VN IEC 61727:2004", 15 },
            {"Sri Lanka---LK IEC 61727:2004", 16 },
            {"ltaly---IT CEI 0-21:2019", 17 },
            {"Morocco---MA IEC 61727:2004", 18 },

            {"ltaly---IT CEI 0-21:2019 ARetti", 20 },
            {"ltaly---ltaly 03", 21 },
            {"South Africa---ZA NRS 097-2-1:2017", 22 },
            {"Belgium---BE C10/11:2019", 23 },

            {"Romania---RO EN 50549-1:2019", 28 },

            {"North America---UL 1741:2010", 30 },
            {"Australia---AU A AS/NZS 4777.2:2020", 31 },
            {"Australia---AU B AS/NZS 4777.2:2020", 32 },
            {"Australia---AU C AS/NZS 4777.2:2020", 33 },
            {"New Zealand---NZ 20 AS/NZS 4777.2:2020", 34 },

            {"Spain---ES UNE 206007-1:2013", 36 },

            {"France---FR VFR 2019", 39 },
            {"France---FR Island 50Hz ", 40 },
            {"France---FR lsland 60Hz", 41 },

            {"Brazil---BR No.140:2022", 44 },

        };
    }
}
