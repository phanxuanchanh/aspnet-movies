using Common.Web;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CategoryBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeDescription;

        public bool IncludeDescription { set { includeDescription = value; } }

        public CategoryBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public CategoryBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeDescription = false;
        }

        private CategoryInfo ToCategoryInfo(Category category)
        {
            if (category == null)
                return null;

            CategoryInfo categoryInfo = new CategoryInfo();
            categoryInfo.ID = category.ID;
            categoryInfo.name = category.name;

            if (includeDescription)
                categoryInfo.description = category.description;

            if (includeTimestamp)
            {
                categoryInfo.createAt = category.createAt;
                categoryInfo.updateAt = category.updateAt;
            }

            return categoryInfo;
        }

        private Category ToCategory(CategoryCreation categoryCreation)
        {
            if (categoryCreation == null)
                throw new Exception("@'categoryCreation' must not be null");

            return new Category
            {
                name = categoryCreation.name,
                description = categoryCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Category ToCategory(CategoryUpdate categoryUpdate)
        {
            if (categoryUpdate == null)
                throw new Exception("@'categoryUpdate' must not be null");

            return new Category
            {
                ID = categoryUpdate.ID,
                name = categoryUpdate.name,
                description = categoryUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<CategoryInfo>> GetCategoriesAsync()
        {
            List<CategoryInfo> categories = null;
            if(includeDescription && includeTimestamp)
                categories = (await db.Categories.ToListAsync())
                    .Select(c => ToCategoryInfo(c)).ToList();
            else if(includeDescription)
                categories = (await db.Categories.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(c => ToCategoryInfo(c)).ToList();
            else if(includeTimestamp)
                categories = (await db.Categories.ToListAsync(c => new { c.ID, c.name, c.createAt, c.updateAt }))
                    .Select(c => ToCategoryInfo(c)).ToList();
            else
                categories = (await db.Categories.ToListAsync(c => new { c.ID, c.name }))
                    .Select(c => ToCategoryInfo(c)).ToList();

            return categories;
        }

        public List<CategoryInfo> GetCategories()
        {
            List<CategoryInfo> categories = null;
            if (includeDescription && includeTimestamp)
                categories = db.Categories.ToList()
                    .Select(c => ToCategoryInfo(c)).ToList();
            else if(includeDescription)
                categories = db.Categories.ToList(c => new { c.ID, c.name, c.description })
                    .Select(c => ToCategoryInfo(c)).ToList();
            else if(includeTimestamp)
                categories = db.Categories.ToList(c => new { c.ID, c.name, c.createAt, c.updateAt })
                    .Select(c => ToCategoryInfo(c)).ToList();
            else
                categories = db.Categories.ToList(c => new { c.ID, c.name })
                    .Select(c => ToCategoryInfo(c)).ToList();

            return categories;
        }

        public async Task<PagedList<CategoryInfo>> GetCategoriesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Category> pagedList = null;
            Expression<Func<Category, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = await db.Categories.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeDescription)
                pagedList = await db.Categories.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description },orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeTimestamp)
                pagedList = await db.Categories.ToPagedListAsync(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Categories.ToPagedListAsync(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<CategoryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCategoryInfo(c)).ToList()
            };
        }

        public PagedList<CategoryInfo> GetCategories(int pageIndex, int pageSize)
        {
            SqlPagedList<Category> pagedList = null;
            Expression<Func<Category, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = db.Categories.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeDescription)
                pagedList = db.Categories.ToPagedList(
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = db.Categories.ToPagedList(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Categories.ToPagedList(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            
            return new PagedList<CategoryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCategoryInfo(c)).ToList()
            };
        }

        public async Task<CategoryInfo> GetCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("@'categoryId' must be greater than 0");

            Category category = null;
            if (includeDescription && includeTimestamp)
                category = await db.Categories.SingleOrDefaultAsync(c => c.ID == categoryId);
            else if(includeDescription)
                category = await db.Categories
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.description }, c => c.ID == categoryId);
            else if(includeTimestamp)
                category = await db.Categories
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.createAt, c.updateAt }, c => c.ID == categoryId);
            else
                category = await db.Categories
                    .SingleOrDefaultAsync(c => new { c.ID, c.name }, c => c.ID == categoryId);

            return ToCategoryInfo(category);
        }

        public CategoryInfo GetCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("@'categoryId' must be greater than 0");

            Category category = null;
            if (includeDescription && includeTimestamp)
                category = db.Categories.SingleOrDefault(c => c.ID == categoryId);
            else if(includeDescription)
                category = db.Categories
                    .SingleOrDefault(c => new { c.ID, c.name, c.description }, c => c.ID == categoryId);
            else if(includeTimestamp)
                category = db.Categories
                   .SingleOrDefault(c => new { c.ID, c.name, c.createAt, c.updateAt }, c => c.ID == categoryId);
            else
                category = db.Categories
                   .SingleOrDefault(c => new { c.ID, c.name }, c => c.ID == categoryId);

            return ToCategoryInfo(category);
        }

        public async Task<List<CategoryInfo>> GetCategoriesByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText;
            if (includeDescription && includeTimestamp)
                commandText = @"Select [Category].* from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else if(includeDescription)
                commandText = @"Select [Category].[ID], [Category].[name], [Category].[description] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else if(includeTimestamp)
                commandText = @"Select [Category].[ID], [Category].[name], [Category].[createAt], [Category].[updateAt] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else
                commandText = @"Select [Category].[ID], [Category].[name] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";

            return await db.Execute_ToListAsync<CategoryInfo>(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
        }

        public List<CategoryInfo> GetCategoriesByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText;
            if (includeDescription && includeTimestamp)
                commandText = @"Select [Category].* from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else if (includeDescription)
                commandText = @"Select [Category].[ID], [Category].[name], [Category].[description] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else if (includeTimestamp)
                commandText = @"Select [Category].[ID], [Category].[name], [Category].[createAt], [Category].[updateAt] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else
                commandText = @"Select [Category].[ID], [Category].[name] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryId] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";

            return db.Execute_ToList<CategoryInfo>(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
        }

        public async Task<CreationState> CreateCategoryAsync(CategoryCreation categoryCreation)
        {
            Category category = ToCategory(categoryCreation);
            if (category.name == null)
                throw new Exception("@'category.name' must not be null");

            int checkExists = (int)await db.Categories.CountAsync(c => c.name == category.name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (category.description == null)
                affected = await db.Categories.InsertAsync(category, new List<string> { "ID", "description" });
            else
                affected = await db.Categories.InsertAsync(category, new List<string> { "ID" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateCategoryAsync(CategoryUpdate categoryUpdate)
        {
            Category category = ToCategory(categoryUpdate);
            if (category.name == null)
                throw new Exception("@'category.name' must not be null");

            int affected;
            if (category.description == null)
                affected = await db.Categories.UpdateAsync(
                    category,
                    c => new { c.name, c.updateAt },
                    c => c.ID == category.ID
                );
            else
                affected = await db.Categories.UpdateAsync(
                    category,
                    c => new { c.name, c.description, c.updateAt },
                    c => c.ID == category.ID
                );

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("@'categoryId' must be greater than 0");

            long categoryDistributionNumber = await db.CategoryDistributions
                .CountAsync(cd => cd.categoryId == categoryId);
            if (categoryDistributionNumber > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.Categories.DeleteAsync(c => c.ID == categoryId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Categories.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
