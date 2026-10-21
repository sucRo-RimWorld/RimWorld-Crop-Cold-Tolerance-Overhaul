# CCTO Plant Cold-Tolerance Evidence Ledger — 2026-10-21

**Status:** literature inventory / evidence mapping only; **NO NEW PARAMETERS APPROVED**. Owner: CCTO Core/Vanilla + Medieval Overhaul. All 78 current `PlantDefs` have a row, **48 finite-death and 30 dormant**. External consumer plant values remain in the consumer repositories. This document supplements [ColdDamageCalibration](ColdDamageCalibration.md); neither is executable XML.

## Evidence rule

- **Source-backed** means a directly reviewed bibliographic page reports the stated **organ/stage/end point**, not necessarily a usable lethal-temperature value. Many references are synthesis or extension material, not original trials.
- **Related proxy** indicates different species, genus or anatomical outcome. Never use as exact plant death temperature; do not assume their numeric ranges for every related Def.
- **Blank** means an adequate matching source **has not yet been collected**, *not* that no research exists.
- Existing `coldDeathTemperature` values are included in [ImplementationTable](ImplementationTable.md) for historical comparison only. They must not be misrepresented as measured LT50 or transferred to onset settings.
- One field observation or an LT50 for buds, leaves, detached tissues, or stored produce is not a universal deterministic death temperature. Always record whether data refer to whole-plant mortality, shoot or crown damage, flowering/yield, fruit injury or winter dormancy.
- **Critical technical distinction:** FAO Table 4.5 reports **stage-dependent ranges of critical frost *damage* temperatures** for field crops, not the duration to death. The separate FAO Table 4.4 is the highest freezing temperature of harvested *produce in storage* and is **excluded** from whole-plant calibration.

## GitHub bibliography retention and verification (2026-10-21)

**This GitHub-tracked document is the canonical reference register for the
CCTO numerical recalibration research.** The **67 unique source IDs** below
carry titles, direct literature/extension URLs, study endpoints and
non-transferability warnings. Source IDs repeated in the later whole-plant
endpoint extraction are cross-references, **not** additional independent
sources. The adjacent 78-Def table is the source-to-plant index, including
explicit no-source rows. Do not silently delete a reference during
recalibration: preserve source IDs in the register and mark later
superseding evidence or corrections.

The *source being listed* does not mean its original full text, numeric
protocol or suitability for lethal-dose fitting is verified. Review status
must accompany any numerical parameter derived from the source:

| Source IDs | Check completed during 2026-10-21 research | Remaining gate |
|---|---|---|
| `ON01` | NMSU breeding-project page reports 90% survival at -13°C for some Japanese-origin accessions and ~-14°C December seedling LT50 for named commercial varieties | The second claim is inconsistent with the Peffley 1981 original (ON03: -8.7/-10.5/-10.8°C). Preserve the discrepancy; do not use -14°C as validated ON03 data. |
| `W01` | [USDA-ARS peer-reviewed publication record](https://www.ars.usda.gov/research/publications/publication/?seqNo115=221641) directly confirms fully acclimated *Triticum aestivum* 25 cultivars, frozen at **-5°C for 15 or 20 weeks**; survival at 15 weeks **0–100%**, at 20 weeks **0–33%**, Norstar/Froid around 33% at 20 weeks | Whole-plant survival under sustained frozen state, but temperature is a chamber treatment of acclimated seedlings. Not equivalent to outdoor air frost mortality; results must not be assigned to spring wheat or shortened to a fixed instant-death threshold. |
| `RY01` | [Abeynayake et al. 2015 original methods and results](https://www.frontiersin.org/journals/plant-science/articles/10.3389/fpls.2015.00329/full) directly confirm *Lolium perenne* two genotypes, 17 days at **7°C** acclimation then **-10°C for 3 hours**, compared with nonacclimated controls | After **7 days at 20°C**, only cold-acclimated plants regrew; the 40/70% numbers are **aboveground biomass regrowth relative to pre-freeze**, not per-plant mortality percentages. |
| `RO01` | Journal article landing/full-text page confirms 2022 research on 17 rose cultivars; field winter damage scored 0–5 with 5 meaning whole-plant death, and a separate modeled whole-plant LT50 | Verify cultivar-by-cultivar numerical LT50 against field exposure and differentiate modeled whole-plant LT50 from stem electrolyte-leakage LT50 |
| `CY01` | Forest Research and Management Organization Japanese abstract confirms winter hardiness of Hinoki cypress around -25°C and strong seasonal/clone variation | The linked detailed PDF is retained but its individual 43-clone table and exposure protocols have not been revalidated in this check; generically named Cypress is not necessarily Hinoki |
| All other source IDs | Earlier sources remain preserved with their reference URLs and endpoint classification | Original protocols, data tables and conversion into CCTO parameters still require individual source-by-source verification. |

For `CY01`, a text-accessible institutional summary is also available at
https://www.ffpri.go.jp/labs/kanko/364-1.html ; the registry retains
the underlying report at
https://www.ffpri.go.jp/pubs/bulletin/351/documents/364-1.pdf .
The two links refer to the same research report, not independent evidence.

**Maintenance rule:** When an input is used to select an XML parameter,
record the source ID, exact organism/cultivar, measured material (whole
plant / regrowth / bud / leaf / tissue), season/acclimation, exposure
temperature/duration and post-thaw observation period; distinguish direct
experiment, a secondary-review quotation and a numerical estimate.
Unsupported onset temperatures, fatal doses and recovery rates remain
unapproved. Historical Git commits preserve superseded and withdrawn
candidate numbers.

## Reviewed source registry

| ID | Source | Type | Reported target and outcome | Constraint |
|---|---|---|---|---|
| `CY01` | [森林総合研究所・高木1993『九州におけるスギ，ヒノキ幼齢木の凍害と防止』](https://www.ffpri.go.jp/pubs/bulletin/351/documents/364-1.pdf) | 一次現地・凍結実験 | Chamaecyparis obtusa(ヒノキ)の自然条件厳冬期耐凍性約-25℃。43クローンから取った枝葉を0℃2週間/-3℃40日順化後、-26/-28/-30℃に凍結し水ざし1か月で生死を判定。-30℃で100%生存のクローン2、80%のクローン2 | **切断枝葉**の水ざし再生/死であり植栽ヒノキ全個体の致死温度ではない。ゲームのCypressがChamaecyparisか未確認。森林総研原文p.36を確認 |
| `AS01` | [Pan et al. 2015, Astragalus membranaceus freezing injury](https://www.sciencedirect.com/science/article/abs/pii/S0098847214001944) | 一次苗実験 | A. membranaceus var. mongholicusの苗、72時間の凍結ストレスで葉/根の酸化障害・イソフラボノイド保護作用を分析。低温前処理で凍害軽減 | Astragalusの種は多数、Defの種不明。葉根の組織障害であって72時間耐える/枯れる温度値ではない |
| `FI01` | [USFS Silvics of North America, Abies balsamea](https://research.fs.usda.gov/silvics/balsam-fir) | USFS林業生態学レビュー | Abies balsameaは冷温帯に適応し冬越しするが、晩発芽苗は越冬生存率が低い;霜柱/雪害/乾燥も苗の死亡原因 | Great FirのモデルがAbiesか要確認。本文に寒波の℃・連続曝露・株死亡率の制御試験はない |
| `PA01` | [Texas A&M AgriLife Extension 2021 freeze assessment](https://www.plantanswers.com/articles/dealing-with-freeze-damage-on-plants.asp) | 大学園芸普及・過去の現地観察 | 1980年代の約0°F(約-17.8℃)のテキサス寒波ではSabal等一部ヤシが生存し、他のヤシは死亡したという観察 | ヤシ科内で属/種差が大きい。厳密な曝露時間・屋内外/葉温・調査標本数がないので全ヤシの致死値不可 |
| `ON01` | [NMSU onion cold-hardiness project](https://onion.nmsu.edu/projects.html) | 二次要約 | 日本由来一部系統-13℃で90%生存。Yellow Grano/Granexの12月苗のLT50は約-14℃と記載。 | **ON03の原著と後者が不一致**。出典・処理条件を確認できるまで-14℃を較正に使用しない。前者は別系統の測定。 |
| `ON03` | [Peffley, Ortiz & Corgan 1981, HortScience 16(6):773–774](https://doi.org/10.21273/HORTSCI.16.6.773) · [原文2頁スキャン](https://pdfs.semanticscholar.org/4f3a/3c0c6125529759251b863550fb12b392e020.pdf) | **原著・表と図を直接確認** | *Allium cepa* Yellow Grano/Yellow Granexの播種後58/74/90日苗。-4℃で一夜保持後、約1℃/h冷却し、群ごとに-6～-14℃で取り出す。21℃以上の温室で3週間後の**発根**を生存判定とする。表1 LT50=-8.7/-10.5/-10.8℃（58日齢の要旨は-8.8℃）。図1に生存確率曲線。 | **各最低温度で一定時間曝露した試験ではない**。冷却過程と到達最低温度が連動するため、単独では低温損傷の時間係数を同定できない。 |
| `ON02` | [UMass Amherst overwintered onion low-tunnel variety trial 2011–12](https://www.umass.edu/agriculture-food-environment/vegetable/special-topics/winter-production-storage/production/variety-trials-for-overwintered-onions) | 大学農場栽培比較 | 保護トンネル内の越冬後生存率、Keepsake/Top Keeper 100%、Bridger/Candy >95%、ネギ類約90% | 穏冬・被覆栽培で最低株温度を記録していない;寒波致死閾値の直接根拠にはならない |
| `RO01` | [Rouet et al. 2022, Mapping Winterhardiness in Garden Roses](https://doi.org/10.21273/JASHS05189-22) | 一次野外調査＋統計モデル | 17園芸バラ品種、冬害尺度の最悪値は株死亡(5)。50%株死亡LT50をロジット推定し冬害指数と比較 | 同属RosaではあるがゲームRoseの品種不明;温度は統計推定/冬全体の気温履歴。茎組織電解質漏出のLT50と株死亡のLT50は別 |
| `RY01` | [Abeynayake et al. 2015, perennial ryegrass acclimation](https://pmc.ncbi.nlm.nih.gov/articles/PMC4428078/) | 一次凍結・再生実験 | Lolium perenne 2遺伝型、7℃/17日順化後-10℃3時間処理し20℃7日回復。順化個体のみ再成長、未順化は再成長せず | 再成長は株生存に近い指標だが個体死亡率%ではない;Haygrass/Grass/Tall grassは種が特定できない |
| `RY02` | [2021 perennial ryegrass waterlogging/acclimation/freezing regrowth](https://pmc.ncbi.nlm.nih.gov/articles/PMC8268127/) | 一次凍結・再生実験 | Lolium perenne 4遺伝型、-6/-8/-10/-12℃で各6時間凍結し21日後の再生を0–9点で判定、RT50算出 | RT50は再生スコア50%低下であって株の死亡率50%ではない |
| `SP01` | [The effects of winter stress on Sphagnum species (2019)](https://www.tandfonline.com/doi/full/10.1080/03736687.2019.1626167) | 一次凍結処理実験（検索結果要旨で確認） | ミズゴケ属6種に-5℃凍結/水没などの比較試験、試験4の生存率は約85%;前処理による差は小さい | 本文への直接アクセスが制限されたため正確な凍結時間・生存尺度は原文再確認。一般的なPlant_Mossの全種に転用禁止 |
| `DA01` | [Cornell University Weed Science dandelion profile](https://cals.cornell.edu/weed-science/weed-profiles/dandelion) | 大学レビュー | Taraxacum officinaleの越冬したロゼットが寒地でも生存、春に地下部から再生 | 具体的な気温・時間・全株死亡率なし;休眠生存の定性的根拠 |
| `DY01` | [Rutgers NJAES FS528 Hemerocallis root-media cold injury](https://njaes.rutgers.edu/fs528/) | 大学普及資料・容器栽培苗試験表 | Hemerocallis 'Joan Senior' の容器培地を-9.4℃にすると、商品性を損なう根傷害/翌季再生低下 | 地植え株の全株死亡ではなく根の生存・再生評価。最適な温度指標は培地温度 |
| `AL01` | [University of Florida IFAS Alocasia spp. extension](https://ask.ifas.ufl.edu/publication/FP033) | 大学園芸普及資料 | アロカシア属の葉は凍結で枯れるが、USDA 8A以南なら春に基部から再生する例がある | 葉死亡と個体全体の死亡を同一視禁止。具体的凍結温度・時間・生存%はない |
| `CL01` | [NMSU 2014 Clivia freeze incident](https://swyg.nmsu.edu/2014/122014.html) | 大学園芸家の観察事例 | Clivia属の鉢で葉が凍結し最終的に個体死亡した観察。対照のアマリリス球根は春再生 | 温度・曝露時間が不明でn=1に近い観察。致死曲線を決めるデータには不適切 |
| `MP01` | [Mura et al. 2025 Acer saccharum bud hardiness](https://pmc.ncbi.nlm.nih.gov/articles/PMC12528974/) | 一次苗木順化試験 | Sugar maple苗木、4℃人工順化と自然越冬後の芽のLT50/芽吹き時期を比較 | 芽と新梢の凍害測定であって樹木全体の死亡ではない; generic Mapleの同一種も未同定 |
| `BR01` | [Oregon State University Blackberry garden guide](https://extension.oregonstate.edu/catalog/ec-1303-growing-blackberries-your-home-garden) | 査読済み大学普及資料 | Rubus属のブラックベリー。多年生地下根/株元と2年生の地上茎が別;寒害は品種/冬前の気温/成長段階で異なる | 凍結による枝・花芽損傷と根株死亡を区別。RimWorld Bramblesの種が同定できない |
| `MO01` | [Zhao et al. 2024 Morus alba bud dormancy and cold resistance](https://pmc.ncbi.nlm.nih.gov/articles/PMC11728178/) | 201集団を用いたGWAS＋遺伝子機能試験 | Morus albaの芽の休眠解除時期と耐寒性の遺伝的相関、MaFAD2過剰発現で芽吹き遅延 | 全株生存率/致死曝露時間を示さない;MO桑の品種は未定 |
| `V03` | [Reyes-Díaz et al. 2006, *Journal of Experimental Botany*, 57:3687–3696, doi:10.1093/jxb/erl125](https://academic.oup.com/jxb/article/57/14/3687/584622) | **一次全株生存実験・LT10/LT50/LT90** | *Arabidopsis thaliana* Columbia, Ler, esk-1, frs-1の鉢植え全株を0～−15℃で各2時間凍結し、復温後10日間に新葉が出た株を生存と判定。各温度あたり30個体、3回の独立測定。大半の系統でLT10とLT90はLT50の周辺で約1～2℃の変動、Lerは−1.5℃付近のLT10に対しLT50約−7.5℃、LT90約−8.5℃という非対称な例もある。 | **種・系統と順化期間ごとに異なる**。図2にLT10/LT50/LT90を示すが、表示された1～2℃は文脈上「中央値周辺の変動」であり一律のLT90−LT10幅やCCTOの共通σを意味しない。 |
| `V04` | [Interrante et al. 2020, *Agricultural & Environmental Letters*, doi:10.1002/ael2.20017](https://acsess.onlinelibrary.wiley.com/doi/10.1002/ael2.20017) | **一次全株生存・ロジスティック回帰** | トールフェスク *Schedonorus arundinaceus* 夏活動/夏休眠2品種。1–2葉期の苗(n=1594)のLT50=−5.7/−5.5℃、5–6葉期の分げつ株(n=1845)のLT50=−6.0/−5.7℃。苗は−4℃以上で全株生存、−8℃以下で全株死亡。凍結約16h、最適条件下で14日回復して生死判定。 | 草一般の値へ転用できない。−4/−8℃は試験内観測で厳密な母集団LT0/LT100ではない。苗齢/品種別の傾きを近似可能だが他の草群へ流用禁止。 |
| `V05` | [Nabati et al. 2026, *BMC Plant Biology* 26:504, doi:10.1186/s12870-026-08352-w](https://link.springer.com/article/10.1186/s12870-026-08352-w) | **一次全株再生生存率・複数温度** | *Cicer arietinum* 8遺伝型。自然条件下で約4か月順化後、約2℃/hで冷却、各0/−6/−10/−12/−14℃を2h保持、4週間後に再成長と緑組織で生死判定。KAKA系統：0℃=100%、−6℃=95%、−10℃=80%、−12℃=70%、−14℃=0%。 | 生存率は系統KAKA・特定の冷却/順化手順の結果。−12～−14℃間の急変は確認できるが、同一温度での異なる曝露時間や耐寒性分布の関数形は同定不能。単一の普遍的σとしない。 |
| `V01` | [Freezing tolerance and genetic loci in *Boechera stricta* (2015)](https://pmc.ncbi.nlm.nih.gov/articles/PMC4416058/) | 一次株生存率/群集遺伝比較 | 凍結後に50%の個体が死亡するLT50を生存率曲線から推定し、低温順化・系統間の耐凍性差を記録。親系統の順化後LT50は短日条件で約-11.3℃/ -8.6℃。 | **異種に係数を転用しない**。LT50は半数死亡の集団統計で、同一遺伝型の全株がちょうどこの温度で死亡するわけではない。今回の用途は個体差モデルの根拠のみ。 |
| `V02` | [Natural variation of *Arabidopsis lyrata* subsp. *petraea* survival under freezing (2018)](https://pmc.ncbi.nlm.nih.gov/articles/PMC6233594/) | 一次株生存率/集団比較 | 2℃で2日低温順化した複数集団を凍結処理し、処理後14日の個体生存率から集団LT50を推定。集団間LT50の範囲は約-8.6～-6.2℃、-7℃では比較対象集団の生存率約76%対34%。 | **集団間の差と集団内の散らばりは別物**。この差をそのまま1種の株個体差分布幅として使わない。 |
| `Q01` | [Cold acclimation of Quercus robur in northern range (2007)](https://www.sciencedirect.com/science/article/abs/pii/S0098847207001876) | 一次生理実験 | 2年生ヨーロッパナラの枝/幹で、9月初旬に耐凍性約-10℃、12月初旬に約-36℃ | 同種不明のCCTO Oak/Great Oakに数値を直輸入しない;樹冠全体の死亡ではない |
| `P02` | [Populus tremula × P. tremuloides, chilling acclimation](https://pubmed.ncbi.nlm.nih.gov/15095138/) | 一次生理実験 | 3か月苗の4℃14日順化で葉の耐凍性が-5.7→-9.8℃;4℃自体では個体は生存し成長ほぼ停止 | 組織耐性と株死亡を分離;交雑ポプラを野生種と同じにしない |
| `S02` | [Rosenblad et al. 2024, montane willow freezing sensitivity](https://nph.onlinelibrary.wiley.com/doi/10.1111/nph.20028) | 一次実験 | Salix spp.春の新梢を凍結させ、挿し穂の再発根で生存可否/LT50評価 | 春芽の再発根で成木樹体死亡とは異なる;冬の順化状態と比較禁止 |
| `S03` | [Lennartsson et al. 2002, willow cold hardening](https://onlinelibrary.wiley.com/doi/full/10.1046/j.1365-3040.2002.00902.x) | 一次実験 | Salix の生育停止後、茎等の蛍光による傷害LT50と順化速度を比較 | 茎組織評価であり各種の厳密な死亡時間は未転用 |
| `T02` | [Luna & Kumar 2008, teak plantations frost treatments](https://indianforester.co.in/index.php/indianforester/article/view/902) | 一次野外試験 | 1～3年生チーク植栽林で霜害後の樹高損失約15.81%を観察 | 曝露気温/時間に対応する全株枯死率の値は示されていない |
| `G03` | [Freezing tolerance of garlic ecotypes, AGRIS indexed trial](https://agris.fao.org/search/es/records/6474734ad02e8fbe07eec822) | 一次試験要旨 | ニンニク4系統、0～-21℃の凍結処理; -15℃で系統/播種時期により生存減20%、26%または大量消失 | 順化した系統の試験でありすべてのニンニクの-15℃死亡値にはならない |
| `G04` | [Hardneck garlic growth/cold injury model (2022)](https://pmc.ncbi.nlm.nih.gov/articles/PMC8967357/) | 温度処理実験とモデル | 第3葉期を0,-5,-10,-15℃に暴露;生存株の葉面積を最大8日測定 | *生存者のみ*葉面積の変化;死亡率は別のG03を使用 |
| `C02` | [Joly & Hahn 1991, Theobroma cacao dark chilling](https://pubmed.ncbi.nlm.nih.gov/14972851/) | 一次生理実験 | カカオ幼苗を一夜4.7～15.8℃に曝露;10℃未満で光合成強く阻害、7日で光合成回復 | 生理低下と株死亡は別;損傷開始温度を直接決定しない |
| `A01` | [Davis et al. 2017, GCB Bioenergy, Agave field trial](https://onlinelibrary.wiley.com/doi/abs/10.1111/gcbb.12324) | 一次野外試験 | Agave tequilana, A. fourcroydes, A. americana — 2013年の5夜連続-6～-8℃の寒波後、A. tequilanaで74%枯死、A. americanaでは寒害/死亡を観察せず | 栽培Agaveの種差でありCCTOの`Plant_Agave`種は特定されていない |
| `A02` | [Agave americana cold-model study 2019, J Exp Bot](https://pmc.ncbi.nlm.nih.gov/articles/PMC6883261/) | 現地観測＋先行細胞実験引用 | A. americana — 50%細胞死温度約-7.4℃、順化温度10℃低下に応じ耐寒限界約1.8℃変化と引用 | 細胞LT50であり全株枯死値ではない |
| `K01` | [Cucurbita pepo seedlings, low-temperature stress 2023](https://pmc.ncbi.nlm.nih.gov/articles/PMC9921430/) | 一次生理実験 | ズッキーニ(C. pepo)幼苗の5℃非凍結処理;酸化ストレスと葉の損傷軽減 | MO Pumpkinと種・品種一致未検証。死亡率や霜害の下限は与えない |
| `M03` | [Creencia & Bramlage 1971, *Plant Physiology* 47:389–392, doi:10.1104/pp.47.3.389](https://pmc.ncbi.nlm.nih.gov/articles/PMC365875/) | **原著・公開要旨直接確認／非凍結チリングによる苗の不可逆死亡** | *Zea mays*幼苗を**+0.3±0.3℃**に曝露。36hで葉の症状は21℃復温後回復。48～60hでは多くが復温で回復するが**一部は倒伏し復活不能**。72hで復温しても症状は回復せず、苗は間もなく死亡。葉/根/茎の損傷と株全体の回復可否を区別。 | 条件別**個体数・死亡割合/LT50は本文公開要旨から不明**。+0.3℃は測定中心値で±0.3℃の変動あり、固定の損傷開始温度を直接決めない。M01の別実験-2/-6℃のLT50に同一苗齢・品種の点として混ぜない。凍結以下に限定する損傷式はこの条件を再現不可。 |
| `M04` | [Jiang et al. 2016, *Frontiers in Plant Science* 7:1477, doi:10.3389/fpls.2016.01477](https://pmc.ncbi.nlm.nih.gov/articles/PMC5054024/) | **原著・実験方法と苗の生存率を確認** | *Zea mays* **30近交系**を3葉期に**−1℃で3h**処理、25℃で3日回復後の個体生存率を比較。最耐寒KR701約**97.3%**、最低Hei8834約**5.01%**。同一曝露でも遺伝型ごとの生存率に著しい差。 | **系統間のばらつきは同一系統内個体差分布の幅ではない**。極端2系統をゲーム上の全Corn株の単一確率分布にまとめない。単一曝露温度・持続時間から温度×時間関数は求まらない。 |
| `M02` | [Ma et al. 2011, Can J Microbiology, ectomycorrhizal fungi](https://pubmed.ncbi.nlm.nih.gov/21491984/) | 一次生理実験 | Cortinarius/Russula/Suillus/Lactarius の菌糸体に+4～-40℃処理;電解質漏出LT50は-7.6～-13.7℃、再成長も検証 | 培養菌糸のLT50; MO Mushroom/ Fly agaricとは種が違い、天然地中菌糸の越冬死亡とは異なる |
| `F01` | [FAO (Snyder & de Melo-Abreu), Frost protection, Vol.1, Table 4.5](https://www.fao.org/4/y7223e/y7223e0a.htm) | 総説/農業資料 | 小麦・大麦・レンズ豆・ケシ・亜麻・ニンジン・キャベツ・トウモロコシ・ジャガイモ・綿・稲・トマト等 — 発芽・開花・結実の低温被害温度帯(℃) | 死亡率、曝露時間、種内差を直接与えない。冬小麦の越冬データではない |
| `F02` | [FAO, Frost protection, Vol.1, Tables 4.7–4.10](https://www.fao.org/4/y7223e/y7223e0a.htm) | 総説/農業資料 | リンゴ・ブドウ・レモン — 花芽の10/90%被害、芽、果実の凍結温度 | 果実・花芽・葉の被害であり樹木全体の死亡ではない |
| `M01` | [Lindsey et al. 2024, Crop Science / Buican 1969](https://acsess.onlinelibrary.wiley.com/doi/10.1002/csc2.21212) | レビュー内の一次実験引用 | トウモロコシ幼苗 — -2℃/48時間と-6℃/2時間で約50%個体死亡 | 原実験Buicanの手法までは本資料のみで完備しない。100%死亡値ではない |
| `W01` | [Skinner & Garland-Campbell 2008, Canadian Journal of Plant Science](https://www.ars.usda.gov/research/publications/publication/?seqNo115=221641) | 実験論文・USDA要旨 | 低温順化した冬小麦25品種 — -5℃/15週で生存率0–100%;20週で0–33% | 品種・順化済み苗に限定;農地の気温を直に入れてはいけない |
| `W03` | [Skinner 2014, *Crop Science* 54:1523–1529, doi:10.2135/cropsci2013.09.0623](https://acsess.onlinelibrary.wiley.com/doi/10.2135/cropsci2013.09.0623) | 一次試験・公開原著要旨確認 | 低温順化した冬小麦6品種、-13.5～-16.5℃に2/3/4時間保持。-10℃以下の総経過時間が325→525分になると平均生存率が約50%低下。しかし**675分の方が生存率が上昇して325分時点と同程度**になる例を報告。 | 冷却経路/低温刺激下の適応反応を著者が示唆。**総曝露時間に単調増加する損傷量だけでは当該結果を再現できない**。品種別原表と各条件の生存率は全文未照合なので係数不可。 |
| `W04` | [Zheng et al. 2018, *Crop Science* 58:1740–1750, doi:10.2135/cropsci2018.01.0014](https://acsess.onlinelibrary.wiley.com/doi/10.2135/cropsci2018.01.0014) | 一次試験・原著要旨確認 | 野外で順化した冬小麦4品種を冬季に各品種の臨界凍結温度で異なる日数保持。追加1日当たりの株死亡率の増分はJing411 **8.6%**、Nongda211 **22.3%**、Zhengmai366 **11.1%**、Yanzhan4110 **9.4%**と報告。分げつ・新葉・古葉では死亡率増分が異なる。 | 原著要旨の「% per day」が割合増減かパーセンテージポイント増減か全文で判定していない。**絶対死亡確率に数値を直用しない**。品種別臨界温度・原表・生存個体数の照合必要。 |
| `W07` | [Pomeroy, Andrews & Fedak (1975), *Canadian Journal of Plant Science* 55:529–535, doi:10.4141/cjps75-079](https://doi.org/10.4141/cjps75-079) | **一次論文書誌/要旨確認・原著詳細表未取得** | 低温順化したKharkov冬小麦の曝露時間と生存を比較。論文要旨に-6℃で5日まで重篤な障害なし、-20℃付近で低温を長くすると損傷増加と記載。**個別生存率はW08の2025年総説による原著引用**：-16℃で1/3/6/12/24時間の生存率80/63/53/33/17%、5日後0%、-6℃で5日後100%。 | W07原著全図表は未直接読了。**二次報告の率を原著から直接転記したと主張しない**。個体数・誤差棒・凍結処理前の冷却期間・株の回復判定・条件について未照合。W08とは同一試験、独立データ2件に数えない。 |
| `W08` | [Arora (2025), *Physiologia Plantarum* 177:e70633, doi:10.1111/ppl.70633](https://pmc.ncbi.nlm.nih.gov/articles/PMC12624383/) | **公開原文閲覧済み・総説／W07の二次再掲** | 原著W07のKharkov小麦を引用し、-16℃での5時間点の生存割合80/63/53/33/17%と5日で0%、-6℃5日で100%と明記。Gusta et al. (1997) の-15℃曝露の50%死亡時間も引用（Siouxland6.5日・Norstar13日）。植物組織のLT50と再生による株死亡LT50が別指標であることを整理。 | 論文表や実測の生存個体数はW08自身の実験ではない。W07とW08は同じデータ系列であり**独立再現ではない**。温度・冷却・順化の研究間比較は限定的。 |
| `W11` | [Bergjord Olsen, Bonesmo & Skjelvåg 2008, *European Journal of Agronomy* 28:321–330, doi:10.1016/j.eja.2007.10.002](https://www.sciencedirect.com/science/article/abs/pii/S1161030107001049) | **一次冬小麦予測モデル／公開要旨確認** | FROSTOLは**土壌深さ約2cmの温度**、積雪、春化発達等で低温順化・脱順化・凍結ストレスによるLT50の季節変化を推定。2品種・2冬季・3地点の実測を用いた開発で、発表時のLT50適合R²**0.84**。 | 生理状態の時間変化に関する独立の確立済みモデルだが、**土壌温度・積雪の追加状態に依存**し、CCTOの単一植物AmbientTemperatureモデルへ係数直用不可。R²は株死亡率予測の汎用精度ではない。 |
| `W12` | [Bergjord Olsen 2018, *Journal of Agronomy and Crop Science* 204:62–73, doi:10.1111/jac.12238](https://onlinelibrary.wiley.com/doi/10.1111/jac.12238) | **FROSTOLの独立地域比較・公開論文要旨確認** | スウェーデンとノルウェー20試験地点の実地越冬クラス「生存/中間/死亡」を予測モデルと比較し、平均のクラス一致は**スウェーデン75%**、**ノルウェー37%**。未知の温度以外の冬害要因が地域差として影響した可能性。 | **クラスの一致率であり株ごとの生存率・LT50測定誤差ではない**。複雑な既存モデルでも地域・条件による予測精度は異なる。FROSTOLをCCTOへ全面移植する実証根拠にはならない。 |
| `W10` | [Husiny et al. (2024), *Canadian Journal of Plant Science* 104:256–264, doi:10.1139/cjps-2023-0143](https://doi.org/10.1139/cjps-2023-0143) · [全文表2公開コピー](https://www.researchgate.net/publication/377906631_Improved_methods_to_estimate_days_and_temperature_to_fifty_percent_mortality_of_winter_wheat_Triticum_aestivum_L_under_low-temperature_flooding_and_ice_encasement) · [著者提供元データ DOI](https://doi.org/10.5683/SP3/YHOZUK) | **原著本文・方法・表2/3読了、株再生を生死判定。元データ未取得** | 3葉1分げつの小麦4品種を3.5℃×4週間順化。試験A：-4℃0/4/8/12/16/20/25/32日保持、氷被覆あり/なし併合のLD50 Norstar33日、CM614 20日、Branson18日。春型AC Carberryは4日で100%死亡。試験B：-4℃予備保持0/7日後に-4→-19℃の各段階を1時間保持し凍結、株再生を温室3週間で判定。原著表2のLT50(day0/day7)：Norstar -13.6/-13.2℃、CM614 -12.7/-12.6℃、Branson -11.9/-11.6℃、AC Carberry -6.6/-2.7℃。 | **同じ個体群の完全な温度×時間死亡率格子ではなく、独立した二試験**。LD50 Norstar33日は観測上限32日を超える外挿値。氷被覆・根圏湿潤と-4℃前処理条件に依存。公開原著が参照するBorealis元データ(doi:10.5683/SP3/YHOZUK)は配布サーバ403で未取得；個体数・条件別生死表を検証したと主張しない。 |
| `W09` | [Gusta & Fowler (1977), *Canadian Journal of Plant Science* 57:213–219, doi:10.4141/cjps77-029](https://doi.org/10.4141/cjps77-029) · [原著本文テキスト](https://www.researchgate.net/publication/250381843_Factors_affecting_the_cold_survival_of_winter_cereals) | **一次論文・原著公開本文の該当節確認** | 野外順化した冬小麦5品種の冠部を徐冷凍結し、最低温度へ到達直後/1h/24h後の再生生存（LD50）を比較。本文は1h保持は到達直後と同等で、24h保持するとLD50が約4℃暖かくなったと報告。冷却速度・再凍結・融解速度も評価。 | 試験は**切除冠部（crown）の再生判定**で全株を土壌ごと凍結する条件ではない。表1から各品種のLD50を正確に転記するには紙面図表の確認が必要。W07の時間生存5点とは独立の実験。 |
| `W06` | [Wu et al. 2024, *Frontiers in Plant Science* 15, doi:10.3389/fpls.2024.1419381](https://www.frontiersin.org/journals/plant-science/articles/10.3389/fpls.2024.1419381/full) | **原著HTML全文・表2～4確認** | 冬小麦Nongda211/Zhengmai366/Yanzhan4110の同品種ごとに、複数の試験温度と1/2/3日の凍結処理を比較。**分げつ死亡率**を気温とのロジスティック曲線で近似（表3）、冠部土壌温度の積算寒度TSE_h(℃·時)/TSE_d(℃·日)で線形回帰（表4）。TSE_hに対する50%分げつ死亡値=106.9/86.5/84.7℃·時、R²=0.83/0.93/0.86。土壌最低温度は指定気温より1/2/3日処理で平均7.5/4.8/4.2℃高い。 | **分げつ死亡であり全株死亡ではない**。冠部測定温度と屋外気温は別。表3 Yanzhan4110の2日式「0.90*Tmin+17.18」は表のLT50=-12.7℃と**代数的に一致しない**（式は約-19.1℃を示す）：誤植/転記確認まで該当係数は計算に使用禁止。実測の個体別生存数は表3/4に含まれない。 |
| `W05` | [Zheng et al. 2018, *Agricultural and Forest Meteorology* 260–261:1–8, doi:10.1016/j.agrformet.2018.05.011](https://doi.org/10.1016/j.agrformet.2018.05.011) · [FAO AGRIS](https://agris.fao.org/search/en/records/65defde17c7033e84bec586f) | 一次試験・要旨確認（本文未取得） | 2年の順化冬小麦試験で**土壌最低温度**-14/-17/-20℃、それぞれ1～10日程度の異なる曝露日数を比較。**土壌最低温度-12℃以下**からの積算凍結度日(FDD)と株・分げつ死亡率を分析し、FDD 1℃·日当たり死亡率がそれぞれ**1.0% / 1.4%**増加と報告。 | 空気温度ではなく**土壌**の観測量。上昇率の統計的定義、交互作用/誤差、シーズン・品種詳細未確認。**-12℃は当該実験の閾値であり汎用小麦の損傷開始温度ではない**。 |
| `W02` | [Skinner et al. 2018, PLOS ONE](https://pmc.ncbi.nlm.nih.gov/articles/PMC6005534/) | 一次実験 | 冬小麦12系統 — 5週間順化後のLT50に光周期/品種差 | LT50は群の50%死亡であり一律枯死温度ではない |
| `B01` | [Wójcik-Jagła & Rapacz 2023, Scientific Reports](https://www.nature.com/articles/s41598-023-47318-y) | 一次実験 | 冬・兼用型大麦 — 順化後の凍結処理、脱順化に伴うLT50・再生の変化 | 春型大麦や実ゲームの大麦全般に自動適用できない |
| `C01` | [NARO 1997, キャベツ幼植物の耐凍性向上と糖蓄積](https://www.naro.go.jp/project/results/laboratory/vegetea/1997/vegetea97-004.html) | 一次研究成果要旨 | キャベツ2–3葉期 — 5℃順化、-8℃凍結後の葉片の電解質漏出 | 葉片損傷であり株全体の死亡率ではない |
| `R01` | [da Cruz et al. 2013, Food and Energy Security](https://onlinelibrary.wiley.com/doi/10.1002/fes3.25) | 学術レビュー | イネ — 約10–13℃の初期生育障害・18–20℃の生殖生長期被害 | 収量/生理障害であり全株死亡閾値ではない |
| `G01` | [Ferguson et al. 2011, Annals of Botany](https://rex.libraries.wsu.edu/esploro/outputs/journalArticle/Dynamic-thermal-time-model-of-cold/99900547082801842) | 一次観測/モデル比較 | ブドウ Cabernet Sauvignon, Chardonnay, Concord — 22年までの芽耐寒観測に基づく順化・脱順化モデル(R²=0.82–0.89) | 芽の耐寒でありブドウ樹の死亡ではない |
| `G02` | [Cornell Finger Lakes Grape Program 2023 winter hardiness](https://blogs.cornell.edu/flxgrapes/2023/02/15/winter-hardiness-update-february-8-2023/) | 実測・普及資料 | ブドウ品種別 — 低温芽LT10/LT50(°F)、実際の気温と比較 | 芽損傷リスク;全樹死と混同禁止 |
| `S01` | [University of Minnesota Extension, Strawberry spring frost](https://extension.umn.edu/agriculture/specialty-crops/commercial-fruit-production/strawberry-farming/protect-strawberries-from-spring-freeze-and-frost) | 普及資料 | イチゴ — 花の開花段階: 蕾は20°F、開花は30°F付近で損傷 | 花の被害と株/クラウンの越冬死は別 |
| `U01` | [LSU AgCenter, Sugarcane freezing field trials](https://www.lsuagcenter.com/portals/communications/publications/agmag/archive/2001/fall/resistance-of-louisiana-sugarcane-to-deterioration-from-freezing-temperatures) | 現地試験記事 | サトウキビ商用品種 — 最低22–24°F、凍結8–15時間、茎組織等の損傷 | 地上部の致死で根株を含む株全体の死亡ではない |
| `P01` | [University of Maine Cooperative Extension, volunteer potato](https://extension.umaine.edu/publications/1090e/) | 普及資料・研究引用 | ジャガイモ地中塊茎 — 27°F未満の長期曝露で多くが除去されるが深い塊茎は越冬可能 | 塊茎の存否であって地上部損傷ではない |
| `T01` | [USFS FEIS, Carnegiea gigantea species review 2021](https://research.fs.usda.gov/feis/species-reviews/cargig) | USFS生態学レビュー | サワロサボテン — -5.6℃未満15–20時間の寒波で重大枯死; -8.3℃/19時間に耐えた報告もある | 矛盾に見える事例は場所・生育段階・条件・組織差;決定的閾値にしない |
| `O01` | [Nobel & de la Barrera 2003, New Phytologist](https://nph.onlinelibrary.wiley.com/doi/10.1046/j.1469-8137.2003.00675.x) | 一次生理実験 | Opuntia ficus-indica — 茎節で-6℃以下の凍害;果実・根で-7℃60分の生体染色損傷 | サワロ・小型球形サボテン等の別種へ直接転用不可 |
| `O02` | [Goldstein & Nobel 1994, Plant Physiology](https://pmc.ncbi.nlm.nih.gov/articles/PMC159246/) | 一次生理実験 | Opuntia humifusaなど — O. humifusa -24℃程度までの耐凍性と順化差 | Opuntiaの種内外差が大きい;全サボテンの共通設定は不適当 |
| `N01` | [Luoranen et al. 2024, New Forests](https://link.springer.com/article/10.1007/s11056-024-10060-y) | 一次実験 | Pinus sylvestris, Betula pendula, Picea abies — 真冬順化苗・芽: マツ-30～-40℃、シラカバ約-30℃、トウヒ最低-50℃ | CCTOのPine/Birch樹種が未指定;苗木と成木の区別必須 |
| `N02` | [USFS FEIS, Pinus rigida review](https://research.fs.usda.gov/feis/species-reviews/pinrig) | 研究引用の生態レビュー | Pinus rigida — 苗木の1月の組織耐凍性-31.7℃など、季節変化 | CCTOのPineは未同定;マツ属の近縁種参考 |
| `H01` | [Penn State Extension, Hop Production](https://extension.psu.edu/hop-production) | 普及資料 | ホップ — 休眠根茎は春先から植付可能;温室移植苗は春霜に弱い | 温度・生存率の直接データなし;同一個体の地上部と根茎を区別 |
| `D01` | [Moso bamboo cold acclimation leaf study 2022](https://pubmed.ncbi.nlm.nih.gov/35723499/) | 一次実験要旨 | Phyllostachys edulis — 葉の低温順化適応機構 | 生存率/全株致死値ではなくP. edulisへの種同定も未完了 |

## Source mapping for every current CCTO plant

An evidence association does **not** imply a verified whole-plant lethal parameter. Empty mappings are deliberately left open. Names are the current CCTO Def map, not newly verified scientific taxonomic names.

| DefName | Current plant | Current cold mode | Source IDs | Collection stage | What is and is not supported |
|---|---|---|---|---|---|
| `Plant_Rice` | Rice | death | `F01`, `R01` | 対象資料あり・端点注意 | 生育段階別被害/低温障害;全株死亡は未測定 |
| `Plant_Potato` | Potato | death | `F01`, `P01` | 対象資料あり・端点注意 | 地上部と地下塊茎は別挙動 |
| `Plant_Corn` | Corn | death | `F01`, `M01`, `M03`, `M04` | 同一種の凍結株生存・非凍結チリング不可逆死・系統差（条件ごとに別） | M01: 別原著の−2℃48h/−6℃2hで各約50%苗死亡。M03: **+0.3±0.3℃で72h後回復不能死亡**、36hなら多く回復。M04: −1℃3h後の30近交系の生存率5～97%は**系統間差**。各実験を同一系統/苗齢の温度×時間データとして合成しない。 |
| `Plant_Strawberry` | Strawberry | death | `S01` | 対象資料あり・端点注意 | 花芽の凍害で株の越冬死ではない |
| `Plant_Haygrass` | Haygrass | death | `RY01`, `RY02` | 別属/群の牧草モデル・再生実験 | Lolium perenne -10℃/3hの順化後の再生を比較;RimWorldの草の全種に数値転用不可 |
| `Plant_Cotton` | Cotton | death | `F01` | 対象資料あり・端点注意 | 霜害閾値の総説のみ |
| `Plant_Devilstrand` | Devilstrand | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Healroot` | Healroot | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Hops` | Hops | dormancy | `H01` | 対象資料あり・端点注意 | 地上部 vs 休眠根茎;定量致死データなし |
| `Plant_Smokeleaf` | Smokeleaf | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Psychoid` | Psychoid | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_TreeCocoa` | Cocoa | death | `C02` | 同種・生理応答のみ | カカオ幼苗の一夜低温で光合成低下/回復;株死亡率ではない |
| `Plant_TreeBamboo` | Bamboo | death | `D01` | 対象資料あり・端点注意 | Moso葉の順化機構;ゲーム樹種特定は未検証 |
| `Plant_TreeBirch` | Birch | dormancy | `N01` | 対象資料あり・端点注意 | Betula pendula実験;ゲーム側のカバノキ種不明 |
| `Plant_TreeCecropia` | Cecropia | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_TreeCypress` | Cypress | dormancy | `CY01` | 同科/近縁種の枝の再生試験 | ヒノキは枝葉-30℃生存の強順化クローン例を含むが、CCTO Cypressの種は不明;樹体死亡ではない |
| `Plant_Dandelion` | Dandelion | dormancy | `DA01` | 同種・越冬生態 | 寒地ロゼット/地下部から再生。定量凍結死は未測定 |
| `Plant_Daylily` | Daylily | dormancy | `DY01` | 同属品種・根傷害 | Hemerocallis培地温度-9.4℃で商品性を失う根傷害;地植え個体の死亡温度でない |
| `Plant_TreeDrago` | Drago tree | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_TreeMaple` | Maple | dormancy | `MP01` | 同属・芽/順化のみ | Acer saccharum苗の芽の凍害LT50、成木個体死ではない |
| `Plant_TreeOak` | Oak | dormancy | `Q01` | 同属・順化耐性 | ヨーロッパナラ2年苗の幹・枝耐凍性;木全体の死亡は別 |
| `Plant_TreePalm` | Palm | death | `PA01` | 同科の寒波生存観察 | 約-18℃の寒波で耐えたヤシ種と死んだ種がある。種不明のPalmへ一律適用不可 |
| `Plant_TreePine` | Pine | death | `N01`, `N02` | 異種比較・数値不可 | 前者はマツ属複数種の参考;Great Firはモミ属でありマツの値を転用禁止 |
| `Plant_TreePoplar` | Poplar | dormancy | `P02` | 同属・葉順化の実験 | 交雑ポプラ葉の耐凍性-5.7→-9.8℃;成木枯死ではない |
| `Plant_Rose` | Rose | dormancy | `RO01` | 同属品種別・全株LT50 | 17品種の冬の死亡/損傷調査を定量化;枝電解質漏出のLT50と全株LT50は別 |
| `Plant_SaguaroCactus` | Saguaro cactus | death | `T01` | 対象資料あり・端点注意 | 同種の生態資料;曝露例の条件差あり |
| `Plant_TreeTeak` | Teak | death | `T02` | 同種・霜害程度のみ | チーク植林1～3年の樹高損失15.81%;死亡率/気温・曝露時間は未確認 |
| `Plant_Timbershroom` | Timbershroom | dormancy | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Tinctoria` | Tinctoria | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_TreeWillow` | Willow | dormancy | `S02`, `S03` | 同属・芽/茎の組織差 | Salixの春新梢再発根と冬順化を区別 |
| `Agarilux` | Agarilux | dormancy | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Agave` | Agave | death | `A01`, `A02` | 同属種実測・種未確定 | Agave 3種の寒波生存差; CCTOのAgaveがどの種か要確認 |
| `Plant_Alocasia` | Alocasia | death | `AL01` | 属単位・葉と株基部 | 葉は凍結で死亡しても基部から再生する。全株致死への換算禁止 |
| `Plant_Ambrosia` | Ambrosia bush | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Astragalus` | Astragalus | dormancy | `AS01` | 同属の葉根損傷研究 | 黄耆苗72時間凍結で組織のストレス変化。DefのAstragalusの同定・致死値なし |
| `Plant_Berry` | Berry bush | dormancy | — | 直接資料未収集 | 植物種・品種/組織/生育段階に対応する資料を検索 |
| `Plant_Brambles` | Brambles | dormancy | `BR01` | Rubus属比較・多年生根 | ブラックベリー枝の冬害と多年生地下茎を区別。種が同定できない |
| `Bryolux` | Bryolux | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Bush` | Bush | dormancy | — | 直接資料未収集 | 植物種・品種/組織/生育段階に対応する資料を検索 |
| `Plant_Chokevine` | Chokevine | dormancy | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Clivia` | Clivia | death | `CL01` | 単例・全株死亡観察 | 葉が凍結した鉢植Cliviaの死亡観察。温度時間不明、数量調整には使わない |
| `Plant_Rafflesia` | Giant rafflesia | death | — | 直接資料未収集 | 植物種・品種/組織/生育段階に対応する資料を検索 |
| `Glowstool` | Glowstool | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `Plant_Grass` | Grass | dormancy | `RY01`, `RY02` | 別属/群の牧草モデル・再生実験 | Lolium perenne -10℃/3hの順化後の再生を比較;RimWorldの草の全種に数値転用不可 |
| `Plant_ShrubLow` | Low shrubs | death | — | 直接資料未収集 | 植物種・品種/組織/生育段階に対応する資料を検索 |
| `Plant_Moss` | Moss | dormancy | `SP01` | 同綱の別属・凍結後生存 | Sphagnum6種の-5℃後生存約85%;ゲームMossとの種類一致・曝露時間未検証 |
| `Plant_PincushionCactus` | Pincushion cactus | death | `O01`, `O02` | 異種比較・数値不可 | 他属のウチワサボテン類でありピンクッションサボテンと同種ではない |
| `Plant_TallGrass` | Tall grass | dormancy | `RY01`, `RY02` | 別属/群の牧草モデル・再生実験 | Lolium perenne -10℃/3hの順化後の再生を比較;RimWorldの草の全種に数値転用不可 |
| `Plant_HealrootWild` | Wild healroot | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_Plant_Onions` | Onion | death | `ON01`, `ON02`, `ON03` | 原著直接確認・株発根の生存LT50 | ON03: 苗齢58/74/90日別LT50=-8.7/-10.5/-10.8℃。-4℃一夜保持と約1℃/h下降冷却の温度系列であり、定温継続時間の試験ではない。ON01の-14℃記載は原著と不一致。 |
| `DankPyon_Plant_Lentils` | Lentil | death | `F01` | 対象資料あり・端点注意 | 発芽/開花/結実の霜害帯 |
| `DankPyon_Plant_Cabbages` | Cabbage | death | `F01`, `C01` | 対象資料あり・端点注意 | 葉片損傷/生育段階別霜害;株死亡との混同禁止 |
| `DankPyon_Plant_Garlic` | Garlic | dormancy | `G03`, `G04` | 同種・株生存/葉成長 | 冷順化したニンニクの系統差;葉面積/死亡を分離 |
| `DankPyon_Plant_Mushrooms` | Mushroom | death | `M02` | 近縁菌類・参考限定 | 4属の培養菌糸LT50と再成長; MOキノコの同定・環境は未確定 |
| `DankPyon_Plant_Wheat` | Wheat | death | `F01`, `W01`, `W02`, `W03`, `W04`, `W05`, `W06`, `W07`, `W08`, `W09`, `W10`, `W11`, `W12` | 冬小麦の株・冠部生存、曝露時間/温度、積算寒度の研究 | W07のKharkov -16℃時間別生存率はW08総説を通じた**二次確認**で原著数表未取得。W09の冠部再生は土壌越冬全株と異なる。W04原著の株死亡回帰を再構成済み。W10は全株再生のLD50/LT50を独立試験で算定、元生死データはBorealis指定だが未取得。W11/12のFROSTOLは土壌温度・雪・春化から季節LT50を予測する既存モデルで、公開されたクラス一致は地域差が大きい。MO Wheatの種類・順化・土壌気温差には一律移植しない。 |
| `DankPyon_Plant_Flax` | Flax | death | `F01` | 対象資料あり・端点注意 | 亜麻は生育段階別被害温度 |
| `DankPyon_Plant_Sugarcane` | Sugarcane | death | `U01` | 対象資料あり・端点注意 | 地上部茎損傷/品質悪化;地下株生存は別 |
| `DankPyon_Plant_Carrots` | Carrot | death | `F01` | 対象資料あり・端点注意 | 発芽段階;根部生存の直接データなし |
| `DankPyon_Plant_Herb` | Herb | death | — | 直接資料未収集 | 植物種・品種/組織/生育段階に対応する資料を検索 |
| `DankPyon_Plant_Tomatoes` | Tomato | death | `F01` | 対象資料あり・端点注意 | 農作物生育段階別霜害;全株致死ではない |
| `DankPyon_Plant_Grape` | Grape | dormancy | `F02`, `G01`, `G02` | 対象資料あり・端点注意 | 品種別芽のLT50・順化曲線;根株/幹は別 |
| `DankPyon_Plant_Pumpkins` | Pumpkin | death | `K01` | 近縁/品種未確定 | Cucurbita pepo幼苗5℃の酸化ストレスで枯死率ではない |
| `DankPyon_Tree_Apple` | Apple | dormancy | `F02` | 対象資料あり・端点注意 | 開花前後の花芽被害で樹体死亡ではない |
| `DankPyon_Tree_Lemon` | Lemon | death | `F02` | 対象資料あり・端点注意 | レモン果実/芽;樹体全体の死亡ではない |
| `DankPyon_Tree_Mulberry` | Mulberry | dormancy | `MO01` | Morus alba芽の順化・分子機構 | 桑の芽吹き/順化研究。個体死亡温度の資料ではない |
| `DankPyon_Tree_GriffonBerry` | Griffon berry | dormancy | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_Plant_Mindwort` | Mindwort | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_Plant_Poppy` | Poppy | death | `F01` | 対象資料あり・端点注意 | Poppy品目は特定種・野生型同定を要する |
| `DankPyon_Plant_Fleawort` | Fleawort | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_Plant_FlyAgaric` | Fly agaric | dormancy | `M02` | 異種菌類・参考限定 | 菌糸体の耐凍性の可能性のみ; ベニテングタケ実測ではない |
| `DankPyon_Plant_MindwortWild` | Wild mindwort | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_Plant_PoppyWild` | Wild poppy | death | `F01` | 対象資料あり・端点注意 | Poppy品目は特定種・野生型同定を要する |
| `DankPyon_Plant_FleawortWild` | Wild fleawort | death | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_Plant_FlyAgaricWild` | Wild fly agaric | dormancy | `M02` | 異種菌類・参考限定 | 菌糸体と地上子実体を区別; ベニテングタケ実測ではない |
| `DankPyon_GreatOak` | Great oak | dormancy | `Q01` | 近縁モデル・参考限定 | Great Oakの実在種同定なし;Quercus roburの枝の数値を流用禁止 |
| `DankPyon_GreatIter` | Great Iter | dormancy | — | 直接資料未収集 | ゲーム架空種/分類不明。実在種への同定を先行 |
| `DankPyon_GreatFir` | Great fir | death | `FI01` | モミ属の越冬生態参考 | Abies balsameaの耐雪・越冬生態。冷害定量死亡率なし;Great Firの種未同定 |
| `DankPyon_GreatWillow` | Great willow | dormancy | `S02`, `S03` | 同属・近縁モデル | Great Willowが実在どのSalix種か未同定 |

## Current scope and next useful searches

- **53 / 78** Defs have a **source or explicit comparative reference** in this first ledger. This is *not* 53 empirically validated species: the category includes weak comparative cross-taxon references. **25 / 78** still lack a mapped source in this pass.
- **High-value direct research gaps**: exact species of CCTO wild trees/bushes (Quercus, Salix, Populus, Cecropia, teak, cocoa), medicinal crops in MO (mindwort/fleawort/herb identification, fly agaric mycelium), onion overwinter tissues, plant roots/crowns vs foliage, any CCTO fictitious plants.
- **Species identity gate**: `Plant_TreePine` is generic pine and `DankPyon_GreatFir` is not pine; none of the published Scots pine/saguaro/Opuntia values are valid global pine/fir/cactus/Agave thresholds.
- **Biology/code interface gate**: decide which endpoint a RimWorld whole `Plant` represents (growth-point death versus leaf injury and loss of yield), and distinguish cold acclimation for viable winter survivors. The draft linear degree-hour formula has not been fitted; **do not issue species parameters, migrate XML, run native tests, or approve Workshop graphs from this registry alone**.

## Whole-plant survival / mortality endpoint extraction (initial high-priority shortlist)

These are the only source rows in this pass whose endpoints **directly involve survival, whole-plant mortality, or subsequent plant regrowth**, rather than just leaf/flower/branch chemistry. Source detail and confounders remain essential.

| Evidence | Subject and treatment | Whole plant endpoint | Use/limitation for numerical fitting |
|---|---|---|---|
| `M01` | Corn seedlings; -2℃ for 48h or -6℃ for 2h | Each gives **about 50% seedling mortality** | Original Buican study quoted in a later review, *not* identical to deterministic CCTO death; stage and recovery interval |
| `W01` | Fully acclimated winter wheat varieties held -5℃ for 15–20 weeks | 20-week survival **0–33%**, cultivar-dependent | Acclimated plants; chamber protocol, variety, thaw and soil/crown matter |
| `A01` | A. tequilana exposed to five nights of -6 to -8℃ in field | About **74% mortality**; A. americana no visible cold injury/death in same field campaign | Different species, microclimate; do not turn generic Agave into either datum |
| `ON01` | NMSU二次要約：日本由来系統-13℃で90%生存 | 商用品種のLT50約-14℃との記述はON03と食い違う | 出典未整理。数値曲線への採用は保留。 |
| `ON03` | タマネギ苗58/74/90日、-4℃一夜保持→1℃/hの冷却で-6～-14℃に到達 | 3週間後の発根による全株LT50=-8.7/-10.5/-10.8℃ | 定温何時間で枯死するかの結果ではない。計算式に使うには冷却履歴の扱いと別の実験が必要。 |
| `ON02` | Overwintering onions under covered low tunnels in a mild winter | **100%** for two named cultivars, **>95%** two others | Field survival but no uncontrolled frost duration/plant-body temperature; cannot infer lethal freezing threshold |
| `RO01` | 17 rose cultivars, field winter damage and frost temperature modeling | Winter mortality category includes **whole dead plant**; model estimates cultivar-specific whole-plant LT50 | Different modelled mortality endpoints and measured twig electrolyte-leakage injury; climate/history/snow effects |
| `RY01` | Two *Lolium perenne* genotypes, 17d at 7℃, then -10℃ 3h, 7d at 20℃ | **Regrowth present** after acclimation; **none** in unacclimated controls | Presence/absence of regrowth, *not* a % mortality curve; seedling stage |
| `RY02` | *L. perenne* after -6 to -12℃ for 6h and 21d recovery | Regrowth scored 0 (dead) through 9 (healthy); RT50 is **50% regrowth-score loss**, not 50% dead plants | Curve of regeneration, not lethal LT50; acclimation and waterlogging treatments |
| `SP01` | Six *Sphagnum* species frozen at -5℃ | Approx **85% survival** in submergence comparison | Exposure duration and survival definition need checking in original article; only Sphagnum, not all mosses |
| `CL01` | Clivia potted specimen accidentally frozen | Plant ultimately died and the foliage perished | Single anecdote, no recorded air/plant minimum or exposure duration; *not suitable* for fitting |

**Do not count** a tissue electrolyte-leakage LT50 as whole-plant LT50; do not automatically treat winter field mortality as cold-only (frost heave, desiccation, pathogens and winter shelter may contribute). `G03` garlic likewise reports some plant survival under accession-specific frost exposures but protocol/detail have not yet been cross-verified; retain it in the registry as provisional and audit its primary experiment before quantitative fitting.


### Boundary of the remaining 25 unmapped Defs

Unmapped names include fictional constructs (Devilstrand, Healroot,
Smokeleaf, Psychoid, Timbershroom, Agarilux, Bryolux, Glowstool, Mindwort,
Fleawort and equivalents), an unconfirmed plant/taxon identity (Tinctoria,
Drago tree, Great Iter, Chokevine), and **broad biologically ambiguous
labels** (Berry bush, Bush, Low shrubs, Herb, Rafflesia-as-generic). These
do **not** have a unique biological death temperature. Before further
species lookup, inspect in-game textures, Def descriptions and mod author
intent. Identifying a plant as a broad ecological type is permissible but
must not claim a verified real-species LT50.

Do not infer that lack of species-specific studies means the organism cannot
survive frost. For real named groups where only tissue endpoints exist, keep
the generic plant death model explicitly approximate and avoid altering
natural biome vegetation until the selected model reproduces repeated winter
survival plausibly.


## Taxonomic and game-plant identity checks (2026-10-21)

**Classification sources below are NOT frost-damage experiments.** They
identify whether a Def can meaningfully be associated with a biological
species. Do **not** count this section as additional verified cold-injury
measurements or raise the 53/78 source-mapping metric because of these links.

| Previously unmapped Def | Directly checkable identity/ecology reference | Cold-death parameter decision |
|---|---|---|
| `Plant_Tinctoria` | [RimWorld Wiki: Tinctoria](https://rimworldwiki.com/wiki/Tinctoria) explicitly describes a genetically engineered dye-secreting flower. It is **not** evidence that this plant is the real dye plant *Isatis tinctoria*. | No same-species empirical LT50 exists. Use clearly marked fictional plant ecology/balance estimates. |
| `Plant_TreeCecropia` | [RimWorld Wiki: Cecropia tree](https://rimworldwiki.com/wiki/Cecropia_tree) calls it a tropical-rainforest plant but leaves real-world species unspecified; [USFS *Cecropia peltata* silvics](https://www.srs.fs.usda.gov/pubs/misc/ag_654/volume_2/cecropia/peltata.htm) gives a possible genus-level analog. | USFS high first-year seedling mortality is **all-cause** forest survival, not cold-induced death. Species not verified; no usable frost LT50 identified. |
| `Plant_Rafflesia` | [Kew Plants of the World Online, *Rafflesia arnoldi*](https://powo.science.kew.org/taxon/urn%3Alsid%3Aipni.org%3Anames%3A316069-1/general-information) describes a tropical **endoparasite of *Tetrastigma*** that persists inside host tissue; only the buds/flowers become visible. | No controlled freezing survival trial for the game Def was verified. A visible Rafflesia flower's death is *not* evidence that the long-lived internal parasite has died. |
| `Plant_TreeDrago` | [RimWorld Wiki: Drago tree](https://rimworldwiki.com/wiki/Drago_tree) describes an arid/desert tree; [*Dracaena draco* cultivated-tree study](https://link.springer.com/article/10.1007/s00468-015-1307-z) documents specimens exposed to frosty Melbourne-area winters. | *Dracaena draco* is a plausible **analogue, not a confirmed Def taxonomy**. The study does not report controlled minimum-temperature/duration whole-tree lethality. |
| `Plant_Berry`, `Plant_Bush`, `Plant_ShrubLow` | Generic game vegetation categories; not biological species names. | Select a representative biome-appropriate analogue only if required; give the derived value **proxy/estimate** status, never a species-level measured claim. |
| `DankPyon_Plant_Herb`, `DankPyon_GreatIter` | The names do not by themselves identify a verified real botanical species. | Check MO Def descriptions or author documentation before selecting any species proxy. |

**Remaining-25 boundary:** absence of a matching botanical species or
a measured survival protocol is a legitimate open finding. Research into
a superficially similar plant does not authorize a numerical parameter.
For real plant tissue studies, distinguish lab-grown whole-plant
root/bud regrowth, natural overwinter survival, fruit/leaf injury, and
chilling-only growth depression. Practical CCTO approximations must
state those assumptions rather than fabricate experimental precision.


### Source-owned identity check: Medieval Overhaul 1.6 Def text

The **uploaded MO 1.6 source package** was examined directly
(`3219596926/1.6/Defs/ThingDefs_Plants/`, 2026-10-21). These entries
are the game's actual labels/descriptions, **not biological literature**.
The Def provides no binomial taxon for the cases below; do not assign
an exact real-world LT50 by guessing from its common name.

| DefName | MO 1.6 XML path suffix | What the game actually specifies | Calibration status |
|---|---|---|---|
| `DankPyon_Plant_Mindwort` and `MindwortWild` | `Plants_Cultivated_Alchemy.xml` | Pale-green leaves and small yellow flowers, used in potent elixirs; **no species ID** | Invented/ambiguous herbal identity. No corresponding measured species selected. |
| `DankPyon_Plant_Fleawort` and `FleawortWild` | `Plants_Cultivated_Alchemy.xml` | A hardy, traditionally medicinal plant; **no genus/species** | The English common name is taxonomically ambiguous; do not automatically assume a particular *Plantago* species. |
| `DankPyon_Plant_Herb` | `Plants_Cultivated_Farm.xml` | Fragrant culinary leaf herb with no botanical name | Generic edible herb. Select a biologically defensible proxy only once owner scope is decided. |
| `DankPyon_Tree_GriffonBerry` | `Plants_Cultivated_Farm.xml` | Fictional small fruit tree whose berries are valued by griffins | Fictional fruit tree; no real-species lethal field measurement. |
| `DankPyon_GreatIter` | `Plants_Wild_DarkForest.xml` | Fictional giant tree, viscous sour fruit, new leaf terrace during hot summers | No real taxonomic equivalent. Generic ecology/gameplay bounds only. |
| `DankPyon_GreatFir` | `Plants_Wild_DarkForest.xml` | Extremely tall evergreen, described as a large cousin of cedars, contrasted against snow | **'Fir' does not establish genus *Abies***. Previous `FI01` balsam-fir literature is ecological analogy only. |

Both cultivated and wild MO alchemy Defs preserve the same descriptive
identity via XML inheritance; they are separate in-game PlantDefs but
not evidence of independently measured biological variants.

The source files were inspected from the original MO archive provided
for this project, not changed or copied into CCTO; MO remains a separate
mod/owner. This identity audit does not increase the cold-evidence
mapping count or authorize new plant XML values.
