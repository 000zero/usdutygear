SELECT 
	category, 
    p.name, 
    CONCAT('="',
		p.model,
        IF(finishes.model IS NOT NULL, CONCAT('-', finishes.model), ''),
        IF(snaps.model IS NOT NULL, CONCAT('-', snaps.model), ''),
        IF(sizes.model IS NOT NULL, CONCAT('-', sizes.model), ''),
        IF(packages.model IS NOT NULL AND packages.model != '', CONCAT('-', packages.model), ''),
        '"') AS model,
	IFNULL(finishes.name, '') AS finish,
    IFNULL(snaps.name, '') AS snap,
    IFNULL(sizes.name, '') AS size,
    IFNULL(packages.name, '') AS package,
    price AS base_price,
    IFNULL(finishes.price_adjustment, '') AS finish_charge,
    IFNULL(snaps.price_adjustment, '') AS snap_charge,
    IFNULL(sizes.price_adjustment, '') AS size_charge,
    IFNULL(packages.price_adjustment, '') AS package_charge,
    (price + IFNULL(finishes.price_adjustment, 0) + IFNULL(snaps.price_adjustment, 0) + IFNULL(sizes.price_adjustment, 0) + IFNULL(packages.price_adjustment, 0)) AS total_price
FROM products AS p
LEFT JOIN product_adjustments AS finishes ON p.model = finishes.product_model AND finishes.type = 'Finish'
LEFT JOIN product_adjustments AS snaps ON p.model = snaps.product_model AND snaps.type = 'Snap'
LEFT JOIN product_adjustments AS sizes ON p.model = sizes.product_model AND sizes.type = 'Size'
LEFT JOIN product_adjustments AS packages ON p.model = packages.product_model AND packages.type = 'Package'
#WHERE p.model = '15'
ORDER BY category, name, model
;