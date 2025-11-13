# Language Support

This mod enables the use of additional languages in a similar way CustomKnight does.

## Adding additional translations

1. Install the mod
2. Download a language pack
    1. I hope to have https://github.com/SFGrenade/LanguageSupport-Repo be a relatively populated collection in the future
3. Run the game once
    1. This generates english translation files
4. Exit the game
5. Go to the `LanguageSupport` folder
    1. This is located next to the `CustomKnight` folder and also next to mod files
6. Extract the language pack here so that the extracted folder has the name of the Language Code of the language it was made for (e.g. `CS` for czech)
    1. See below for a table of available Language Codes.
7. Start the game
8. The language should now be selectable in the options

## Making additional translations

1. Install the mod
2. Run the game once
    1. This generates english translation files
3. Exit the game
4. Go to the `LanguageSupport` folder
    1. This is located next to the `CustomKnight` folder and also next to mod files
5. Copy the `EN` folder and name it to the Language Code of the language you want to translate to (e.g. `CS` for czech)
    1. See below for a table of available Language Codes.
6. Translate the files
7. Open `MainMenu.txt`
8. Add a line at the top that says `<entry name="LANG_##">Language Name Here</entry>`, which `##` being the same Language Code from step 5
9. Start the game
10. The language should now be selectable in the options
11. Please open an issue or pull request at https://github.com/SFGrenade/LanguageSupport-Repo, so others can use the translation as well!

## Table of available Language Codes

| Language Code | Name | Vanilla? | Notes |
|-|-|-|-|
| `N` | ? | no | I think it's meant as None |
| `AA` | Afar | no | aka: `aa`, `aar`, `aar` |
| `AB` | Abkhazian | no | aka: `ab`, `abk`, `abk` |
| `AF` | Afrikaans | no | aka: `af`, `afr`, `afr` |
| `AM` | Anharic | no | aka: `am`, `amh`, `amh` |
| `AR` | Arabic | no | aka: `ar`, `ara`, `ara`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#ara |
| `AR_SA` | Arabic-Macro | no | ^ |
| `AR_EG` | Arabic-Macro | no | ^ |
| `AR_DZ` | Arabic-Macro | no | ^ |
| `AR_YE` | Arabic-Macro | no | ^ |
| `AR_JO` | Arabic-Macro | no | ^ |
| `AR_KW` | Arabic-Macro | no | ^ |
| `AR_BH` | Arabic-Macro | no | ^ |
| `AR_IQ` | Arabic-Macro | no | ^ |
| `AR_MA` | Arabic-Macro | no | ^ |
| `AR_LY` | Arabic-Macro | no | ^ |
| `AR_OM` | Arabic-Macro | no | ^ |
| `AR_SY` | Arabic-Macro | no | ^ |
| `AR_LB` | Arabic-Macro | no | ^ |
| `AR_AE` | Arabic-Macro | no | ^ |
| `AR_QA` | Arabic-Macro | no | ^ |
| `AS` | Assamese | no | aka: `as`, `asm`, `asm` |
| `AY` | Aymara | no | aka: `ay`, `aym`, `aym` + 2 |
| `AZ` | Azerbaijani | no | aka: `az`, `aze`, `aze` + 2 |
| `BA` | Bashkir | no | aka: `ba`, `bak`, `bak` |
| `BE` | Belarusian | no | aka: `be`, `bel`, `bel` |
| `BG` | Bulgarian | no | aka: `bg`, `bul`, `bul` |
| `BH` | Bihari Languages | no | https://en.wikipedia.org/wiki/Bihari_languages |
| `BI` | Bislama | no | aka: `bi`, `bis`, `bis` |
| `BN` | Bengali | no | aka: `bn`, `ben`, `ben` |
| `BO` | Tibetan | no | aka: `bo`, `bod`, `tib` |
| `BR` | Breton | no | aka: `br`, `bre`, `bre` |
| `CA` | Catalan, Valencian | no | aka: `ca`, `cat`, `cat` |
| `CO` | Corsican | no | aka: `co`, `cos`, `cos` |
| `CS` | Czech | no | aka: `cs`, `ces`, `cze` |
| `CY` | Welsh | no | aka: `cy`, `cym`, `wel`, `cym` |
| `DA` | Danish | no | aka: `da`, `dan`, `dan` |
| `DE` | German | YES | aka: `de`, `deu`, `ger`, `deu`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#deu |
| `DE_AT` | German-Macro | no | ^ |
| `DE_LI` | German-Macro | no | ^ |
| `DE_CH` | German-Macro | no | ^ |
| `DE_LU` | German-Macro | no | ^ |
| `DZ` | Dzongkha | no | aka: `dz`, `dzo`, `dzo` |
| `EL` | Greek, Modern (1453–) | no | aka: `el`, `ell`, `gre`, `ell` |
| `EN` | English | YES | aka: `en`, `eng`, `eng`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#eng |
| `EN_US` | English-Macro | no | ^ |
| `EN_AU` | English-Macro | no | ^ |
| `EN_NZ` | English-Macro | no | ^ |
| `EN_ZA` | English-Macro | no | ^ |
| `EN_CB` | English-Macro | no | ^ |
| `EN_TT` | English-Macro | no | ^ |
| `EN_GB` | English-Macro | no | ^ |
| `EN_CA` | English-Macro | no | ^ |
| `EN_IE` | English-Macro | no | ^ |
| `EN_JM` | English-Macro | no | ^ |
| `EN_BZ` | English-Macro | no | ^ |
| `EO` | Esperanto | no | aka: `eo`, `epo`, `epo` |
| `ES` | Spanish, Castilian | YES | aka: `es`, `spa`, `spa`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#spa |
| `ES_MX` | Spanish-Macro | no | ^ |
| `ES_CR` | Spanish-Macro | no | ^ |
| `ES_DO` | Spanish-Macro | no | ^ |
| `ES_CO` | Spanish-Macro | no | ^ |
| `ES_AR` | Spanish-Macro | no | ^ |
| `ES_CL` | Spanish-Macro | no | ^ |
| `ES_PY` | Spanish-Macro | no | ^ |
| `ES_SV` | Spanish-Macro | no | ^ |
| `ES_NI` | Spanish-Macro | no | ^ |
| `ES_GT` | Spanish-Macro | no | ^ |
| `ES_PA` | Spanish-Macro | no | ^ |
| `ES_VE` | Spanish-Macro | no | ^ |
| `ES_PE` | Spanish-Macro | no | ^ |
| `ES_EC` | Spanish-Macro | no | ^ |
| `ES_UY` | Spanish-Macro | no | ^ |
| `ES_BO` | Spanish-Macro | no | ^ |
| `ES_HN` | Spanish-Macro | no | ^ |
| `ES_PR` | Spanish-Macro | no | ^ |
| `ET` | Estonian | no | aka: `et`, `est`, `est` + 2 |
| `EU` | Basque | no | aka: `eu`, `eus`, `baq`, `eus` |
| `FA` | Persian | no | aka: `fa`, `fas`, `per`, `fas` + 2 |
| `FI` | Finnish | no | aka: `fi`, `fin`, `fin` |
| `FJ` | Fijian | no | aka: `fj`, `fij`, `fij` |
| `FO` | Faroese | no | aka: `fo`, `fao`, `fao` |
| `FR` | French | YES | aka: `fr`, `fra`, `fre`, `fra`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#fra |
| `FR_BE` | French-Macro | no | ^ |
| `FR_CH` | French-Macro | no | ^ |
| `FR_CA` | French-Macro | no | ^ |
| `FR_LU` | French-Macro | no | ^ |
| `FY` | Western Frisian | no | aka: `fy`, `fry`, `fry` |
| `GA` | Irish | no | aka: `ga`, `gle`, `gle` |
| `GD` | Gaelic, Scottish Gaelic | no | aka: `gd`, `gla`, `gla` |
| `GL` | Galician | no | aka: `gl`, `glg`, `glg` |
| `GN` | Guarani | no | aka: `gn`, `grn`, `grn` + 5 |
| `GU` | Gujarati | no | aka: `gu`, `guj`, `guj` |
| `HA` | Hausa | no | aka: `ha`, `hau`, `hau` |
| `HI` | Hindi | no | aka: `hi`, `hin`, `hin` |
| `HE` | Hebrew | no | aka: `he`, `heb`, `heb` |
| `HR` | Croatian | no | aka: `hr`, `hrv`, `hrv` |
| `HU` | Hungarian | no | aka: `hu`, `hun`, `hun` |
| `HY` | Armenian | no | aka: `hy`, `hye`, `arm`, `hye` |
| `IA` | Interlingua (International Auxiliary Language Association) | no | aka: `ia`, `ina`, `ina` |
| `ID` | Indonesian | no | aka: `id`, `ind`, `ind` |
| `IE` | Interlingue, Occidental | no | aka: `ie`, `ile`, `ile` |
| `IK` | Inupiaq | no | aka: `ik`, `ipk`, `ipk` + 2 |
| `IN` | Indonesian | no | It's so good they made it twice |
| `IS` | Icelandic | no | aka: `is`, `isl`, `ice`, `isl` |
| `IT` | Italian | YES | aka: `it`, `ita`, `ita`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#ita |
| `IT_CH` | Italian-Macro | no | ^ |
| `IU` | Inuktitut | no | aka: `iu`, `iku`, `iku` + 2 |
| `IW` | Hebrew | no | It's so good they made it twice |
| `JA` | Japanese | YES | aka: `ja`, `jpn`, `jpn` |
| `JI` | Yiddish | no | It's so good they made it twice |
| `JW` | Javanese | no | Should've been `JV` |
| `KA` | Georgian | no | aka: `ka`, `kat`, `geo`, `kat` |
| `KK` | Kazakh | no | aka: `kk`, `kaz`, `kaz` |
| `KL` | Kalaallisut, Greenlandic | no | aka: `kl`, `kal`, `kal` |
| `KM` | Central Khmer | no | aka: `km`, `khm`, `khm` |
| `KN` | Kannada | no | aka: `kn`, `kan`, `kan` |
| `KO` | Korean | YES | aka: `ko`, `kor`, `kor` |
| `KS` | Kashmiri | no | aka: `ks`, `kas`, `kas` |
| `KU` | Kurdish | no | aka: `ku`, `kur`, `kur` + 3 |
| `KY` | Kyrgyz, Kirghiz | no | aka: `ky`, `kir`, `kir` |
| `LA` | Latin | no | aka: `la`, `lat`, `lat` |
| `LN` | Lingala | no | aka: `ln`, `lin`, `lin` |
| `LO` | Lao | no | aka: `lo`, `lao`, `lao` |
| `LT` | Lithuanian | no | aka: `lt`, `lit`, `lit` |
| `LV` | Latvian | no | aka: `lv`, `lav`, `lav` + 2 |
| `MG` | Malagasy | no | aka: `mg`, `mlg`, `mlg` + 11 |
| `MI` | Maori | no | aka: `mi`, `mri`, `mao`, `mri` |
| `MK` | Macedonian | no | aka: `mk`, `mkd`, `mac`, `mkd` |
| `ML` | Malayalam | no | aka: `ml`, `mal`, `mal` |
| `MN` | Mongolian | no | aka: `mn`, `mon`, `mon` + 2 |
| `MO` | Moldovian, Moldovan | no | It's so good they made it twice? |
| `MR` | Marathi | no | aka: `mr`, `mar`, `mar` |
| `MS` | Malay | no | aka: `ms`, `msa`, `may`, `msa` |
| `MT` | Maltese | no | aka: `mt`, `mlt`, `mlt` |
| `MY` | Burmese | no | aka: `my`, `mya`, `bur`, `mya` |
| `NA` | Nauru | no | aka: `na`, `nau`, `nau` |
| `NE` | Nepali | no | aka: `ne`, `nep`, `nep` + 2 |
| `NL` | Dutch, Flemish | no | aka: `nl`, `nld`, `dut`, `nld`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#nld |
| `NL_BE` | Dutch-Macro | no | ^ |
| `NO` | Norwegian | no | aka: `no`, `nor`, `nor` + 2 |
| `OC` | Occitan | no | aka: `oc`, `oci`, `oci` |
| `OM` | Oromo | no | aka: `om`, `orm`, `orm` + 4 |
| `OR` | Oriya | no | aka: `or`, `ori`, `ori` + 2 |
| `PA` | Punjabi, Panjabi | no | aka: `pa`, `pan`, `pan` |
| `PL` | Polish | no | aka: `pl`, `pol`, `pol` |
| `PS` | Pashto, Pushto | no | aka: `ps`, `pus`, `pus` + 3 |
| `PT` | Portuguese | YES | No clue why it's brasilian portuguese when PT_BR is a thing |
| `PT_BR` | Portuguese-Macro | no | In theory this should've been brasilian portuguese |
| `QU` | Quechua | no | aka: `qu`, `que`, `que` + 43 |
| `RM` | Romansh | no | aka: `rm`, `roh`, `roh` |
| `RN` | Rundi | no | aka: `rn`, `run`, `run` |
| `RO` | Romanian, Moldavian, Moldovan | no | aka: `ro`, `ron`, `rum`, `ron` |
| `RO_MO` | Romanian-Macro | no | ^, maybe also Moldova lol |
| `RU` | Russian | YES | aka: `ru`, `rus`, `rus`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#rus |
| `RU_MO` | Russian-Macro | no | ^ |
| `RW` | Kinyarwanda | no | aka: `rw`, `kin`, `kin` |
| `SA` | Sanskrit | no | aka: `sa`, `san`, `san` + 2 |
| `SD` | Sindhi | no | aka: `sd`, `snd`, `snd` |
| `SG` | Sango | no | aka: `sg`, `sag`, `sag` |
| `SH` | Serbo-Croatian | no | https://en.wikipedia.org/wiki/Serbo-Croatian#ISO_classification |
| `SI` | Sinhala, Sinhalese | no | aka: `si`, `sin`, `sin` |
| `SK` | Slovak | no | aka: `sk`, `slk`, `slo`, `slk` |
| `SL` | Slovenian | no | aka: `sl`, `slv`, `slv` |
| `SM` | Samoan | no | aka: `sm`, `smo`, `smo` |
| `SN` | Shona | no | aka: `sn`, `sna`, `sna` |
| `SO` | Somali | no | aka: `so`, `som`, `som` |
| `SQ` | Albanian | no | aka: `sq`, `sqi`, `alb`, `sqi` |
| `SR` | Serbian | no | aka: `sr`, `srp`, `srp` |
| `SS` | Swati | no | aka: `ss`, `ssw`, `ssw` |
| `ST` | Southern Sotho | no | aka: `st`, `sot`, `sot` |
| `SU` | Sundanese | no | aka: `su`, `sun`, `sun` |
| `SV` | Swedish | no | aka: `sv`, `swe`, `swe`. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#swe |
| `SV_FI` | Swedish-Macro | no | ^ |
| `SW` | Swahili | no | aka: `sw`, `swa`, `swa` + 2 |
| `TA` | Tamil | no | aka: `ta`, `tam`, `tam` |
| `TE` | Telugu | no | aka: `te`, `tel`, `tel` |
| `TG` | Tajik | no | aka: `tg`, `tgk`, `tgk` |
| `TH` | Thai | no | aka: `th`, `tha`, `tha` |
| `TI` | Tigrinya | no | aka: `ti`, `tir`, `tir` |
| `TK` | Turkmen | no | aka: `tk`, `tuk`, `tuk` |
| `TL` | Tagalog | no | aka: `tl`, `tgl`, `tgl` |
| `TN` | Tswana | no | aka: `tn`, `tsn`, `tsn` |
| `TO` | Tonga (Tonga Islands) | no | aka: `to`, `ton`, `ton` |
| `TR` | Turkish | no | aka: `tr`, `tur`, `tur` |
| `TS` | Tsonga | no | aka: `ts`, `tso`, `tso` |
| `TT` | Tatar | no | aka: `tt`, `tat`, `tat` |
| `TW` | Twi | no | aka: `tw`, `twi`, `twi` |
| `UG` | Uighur, Uyghur | no | aka: `ug`, `uig`, `uig` |
| `UK` | Ukrainian | no | aka: `uk`, `ukr`, `ukr` |
| `UR` | Urdu | no | aka: `ur`, `urd`, `urd` |
| `UZ` | Uzbek | no | aka: `uz`, `uzb`, `uzb` + 2 |
| `VI` | Vietnamese | no | aka: `vi`, `vie`, `vie` |
| `VO` | Volapük | no | aka: `vo`, `vol`, `vol` |
| `WO` | Wolof | no | aka: `wo`, `wol`, `wol` |
| `XH` | Xhosa | no | aka: `xh`, `xho`, `xho` |
| `YI` | Yiddish | no | aka: `yi`, `yid`, `yid` + 2 |
| `YO` | Yoruba | no | aka: `yo`, `yor`, `yor` |
| `ZA` | Zhuang, Chuang | no | aka: `za`, `zha`, `zha` + 16 |
| `ZH` | Chinese | YES | aka: `zh`, `zho`, `chi`, `zho` + 19. Refer to https://en.wikipedia.org/wiki/ISO_639_macrolanguage#zho |
| `ZH_TW` | Chinese-Macro | no | ^ |
| `ZH_HK` | Chinese-Macro | no | ^ |
| `ZH_CN` | Chinese-Macro | no | ^ |
| `ZH_SG` | Chinese-Macro | no | ^ |
| `ZU` | Zulu | no | aka: `zu`, `zul`, `zul` |

# EUPL
                      Copyright (c) 2025 SFGrenade
                      Licensed under the EUPL-1.2
https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12
