SELECT tarih,[1],[2],[3],[4],[5] FROM
(SELECT [City],[Count],CAST([CovidDate] as date) as tarih
 FROM Covids) as covidT
 PIVOT
 (
	SUM(Count) FOR City IN([1],[2],[3],[4],[5])
 ) as pTable
 Order By tarih asc