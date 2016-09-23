SELECT 
	models.model AS singleModel,  
    IF(packages.model IS NULL, NULL, CONCAT(models.model, '-', packages.model)) AS packageModel
FROM (
	SELECT 
		CONCAT(
			p.model,
			IF(finishes.model IS NOT NULL, CONCAT('-', finishes.model), ''),
			IF(buckles.model IS NOT NULL, CONCAT('-', buckles.model), ''),
			IF(snaps.model IS NOT NULL, CONCAT('-', snaps.model), ''),
			IF(sizes.model IS NOT NULL, CONCAT('-', sizes.model), '')
			#IF(packages.model IS NOT NULL AND packages.model != '', CONCAT('-', packages.model), '')
		) AS model
	FROM products AS p
	LEFT JOIN product_adjustments AS finishes ON p.model = finishes.product_model AND finishes.type = 'Finish'
	LEFT JOIN product_adjustments AS buckles ON p.model = buckles.product_model AND buckles.type = 'Buckle'
	LEFT JOIN product_adjustments AS snaps ON p.model = snaps.product_model AND snaps.type = 'Snap'
	LEFT JOIN product_adjustments AS sizes ON p.model = sizes.product_model AND sizes.type = 'Size'
	LEFT JOIN product_adjustments AS innerLiners ON p.model = innerLiners.product_model AND innerLiners.type = 'Inner Liner'
	WHERE p.model = '66'
) AS models
LEFT JOIN product_packages packages ON models.model RLIKE packages.applicable_model
ORDER BY models.model
;