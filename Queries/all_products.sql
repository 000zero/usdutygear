SELECT 
	results3.category,
    results3.name,
    CONCAT('="', IF(packages.model IS NOT NULL AND splitter.pos = 1, CONCAT(results3.model, '-', packages.model), results3.model), '"') AS model,
    IFNULL(results3.finish, '') AS finish,
    IFNULL(results3.snap, '') AS snap,
    IFNULL(results3.buckle, '') AS buckle,
    IFNULL(results3.innerLiner, '') AS innerLiner,
    IFNULL(results3.size, '') AS size,
    IF(packages.name IS NOT NULL AND splitter.pos = 1, packages.name, '') AS package,
    results3.base_price,
    IFNULL(results3.finish_charge, '') AS finish_charge,
    IFNULL(results3.snap_charge, '') AS snap_charge,
    IFNULL(results3.buckle_charge, '') AS buckle_charge,
    IFNULL(results3.innerLiner_charge, '') AS innerLiner_charge,
    IFNULL(results3.size_charge, '') AS size_charge,
    IF(packages.price IS NOT NULL AND splitter.pos = 1, packages.price, '') AS package_charge,
    IF(packages.price IS NOT NULL AND splitter.pos = 1, 
		packages.price, 
        results3.base_price 
			+ IFNULL(results3.finish_charge, 0) 
            + IFNULL(results3.snap_charge, 0) 
            + IFNULL(results3.buckle_charge, 0) 
            + IFNULL(results3.innerLiner_charge, 0) 
            + IFNULL(results3.size_charge, 0)
	) AS total_price
FROM (
	SELECT results2.*, sizes.price_adjustment AS size_charge
	FROM (
		SELECT results.*, IF(sizes.product_model IS NULL, 0, 1) AS has_special_price, sizes.product_model AS size_product_model
		FROM (
			SELECT DISTINCT
				category, 
				p.name, 
				CONCAT(
					p.model,
					IF(finishes.model IS NOT NULL, CONCAT('-', finishes.model), ''),
					IF(buckles.model IS NOT NULL, CONCAT('-', buckles.model), ''),
					IF(snaps.model IS NOT NULL, CONCAT('-', snaps.model), ''),
					IF(innerLiners.model IS NOT NULL, CONCAT('-', innerLiners.model), ''),
					IF(sizes.model IS NOT NULL, CONCAT('-', sizes.model), '')
				) AS model,
				p.model AS product_model,
				IFNULL(finishes.name, NULL) AS finish,
				IFNULL(snaps.name, NULL) AS snap,
                IFNULL(buckles.name, NULL) AS buckle,
                IFNULL(innerLiners.name, NULL) AS innerLiner,
				IFNULL(sizes.name, NULL) AS size,
				price AS base_price,
				IFNULL(finishes.price_adjustment, NULL) AS finish_charge,
				IFNULL(snaps.price_adjustment, NULL) AS snap_charge,
				IFNULL(buckles.price_adjustment, NULL) AS buckle_charge,
				IFNULL(innerLiners.price_adjustment, NULL) AS innerLiner_charge
			FROM products AS p
			LEFT JOIN product_adjustments AS finishes ON p.model = finishes.product_model AND finishes.type = 'Finish'
			LEFT JOIN product_adjustments AS buckles ON p.model = buckles.product_model AND buckles.type = 'Buckle'
			LEFT JOIN product_adjustments AS snaps ON p.model = snaps.product_model AND snaps.type = 'Snap'
			LEFT JOIN product_adjustments AS innerLiners on p.model = innerLiners.product_model AND innerLiners.type = 'Inner Liner'
			LEFT JOIN product_adjustments AS sizes ON p.model = sizes.product_model AND sizes.type = 'Size'
			#WHERE p.model = '556'
		) AS results
		LEFT JOIN product_adjustments AS sizes 
			ON results.product_model = sizes.product_model 
			AND results.size = sizes.model
			AND sizes.type = 'Size' 
			AND sizes.dependent_models IS NOT NULL AND results.model RLIKE sizes.dependent_models
	) AS results2
	LEFT JOIN product_adjustments AS sizes 
		ON results2.product_model = sizes.product_model 
		AND results2.size = sizes.model
		AND sizes.type = 'Size' 
		AND ((results2.has_special_price AND results2.model RLIKE sizes.dependent_models) OR (NOT results2.has_special_price AND sizes.dependent_models IS NULL))
) AS results3
LEFT JOIN product_packages AS packages
    ON results3.product_model = packages.product_model
	AND results3.model RLIKE packages.applicable_model
LEFT JOIN (SELECT 1 AS pos UNION SELECT 2 AS pos) AS splitter ON packages.model IS NOT NULL
ORDER BY category, results3.name, results3.model
;