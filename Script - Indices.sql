CREATE INDEX IDX_Article_01
ON Article(Description,
		   BarCode,
		   Price,
		   Status,
		   CategoryId);

CREATE INDEX IDX_Category_01
ON Category(Id,
			Name);

CREATE INDEX IDX_Order_Detail_01
ON Order_Detail(OrderId,
				ArticleId);

CREATE INDEX IDX_Order_01
ON [Order](Id,
		   Status);



/*
DROP INDEX Article.IDX_Article_01;
DROP INDEX Category.IDX_Category_01;
DROP INDEX Order_Detail.IDX_Order_Detail_01;
DROP INDEX [Order].IDX_Order_01;
*/