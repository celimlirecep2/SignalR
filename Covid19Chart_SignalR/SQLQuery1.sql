USE CovidHubDB
SELECT tarih,[1],[2],[3],[4],[5] FROM
(SELECT [City],[Count],substring([CovidDate], 6, 2)  as tarih
 FROM dbo.Covids) as covidT
 PIVOT
 (
	SUM(Count) FOR City IN([1],[2],[3],[4],[5])
 ) as pTable