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
    public class TagBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeDescription;

        public bool IncludeDescription { set { includeDescription = value; } }

        public TagBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public TagBLL(BusinessLogicLayer bll)
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

        private TagInfo ToTagInfo(Tag tag)
        {
            if (tag == null)
                return null;

            TagInfo tagInfo = new TagInfo();
            tagInfo.ID = tag.ID;
            tagInfo.name = tag.name;

            if (includeDescription)
                tagInfo.description = tag.description;

            if (includeTimestamp)
            {
                tagInfo.createAt = tag.createAt;
                tagInfo.updateAt = tag.updateAt;
            }

            return tagInfo;
        }

        private Tag ToTag(TagCreation tagCreation)
        {
            if (tagCreation == null)
                throw new Exception("@'tagCreation' must not be null");

            return new Tag
            {
                name = tagCreation.name,
                description = tagCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Tag ToTag(TagUpdate tagUpdate)
        {
            if (tagUpdate == null)
                throw new Exception("@'tagUpdate' must not be null");

            return new Tag
            {
                ID = tagUpdate.ID,
                name = tagUpdate.name,
                description = tagUpdate.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<TagInfo>> GetTagsAsync()
        {
            List<TagInfo> tags = null;
            if (includeDescription && includeTimestamp)
                tags = (await db.Tags.ToListAsync()).Select(t => ToTagInfo(t)).ToList();
            else if (includeDescription)
                tags = (await db.Tags.ToListAsync(t => new { t.ID, t.name, t.description }))
                    .Select(t => ToTagInfo(t)).ToList();
            else if (includeTimestamp)
                tags = (await db.Tags.ToListAsync(t => new { t.ID, t.name, t.createAt, t.updateAt }))
                    .Select(t => ToTagInfo(t)).ToList();
            else
                tags = (await db.Tags.ToListAsync(t => new { t.ID, t.name }))
                    .Select(t => ToTagInfo(t)).ToList();

            return tags;
        }

        public List<TagInfo> GetTags()
        {
            List<TagInfo> tags = null;
            if (includeDescription && includeTimestamp)
                tags = db.Tags.ToList().Select(t => ToTagInfo(t)).ToList();
            else if (includeDescription)
                tags = db.Tags.ToList(t => new { t.ID, t.name, t.description })
                    .Select(t => ToTagInfo(t)).ToList();
            else if (includeTimestamp)
                tags = db.Tags.ToList(t => new { t.ID, t.name, t.createAt, t.updateAt })
                    .Select(t => ToTagInfo(t)).ToList();
            else
                tags = db.Tags.ToList(t => new { t.ID, t.name })
                    .Select(t => ToTagInfo(t)).ToList();

            return tags;
        }

        public async Task<PagedList<TagInfo>> GetTagsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Tag> pagedList = null;
            Expression<Func<Tag, object>> orderBy = t => new { t.ID };
            if (includeDescription && includeTimestamp)
                pagedList = await db.Tags.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = await db.Tags.ToPagedListAsync(
                    t => new { t.ID, t.name, t.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = await db.Tags.ToPagedListAsync(
                    t => new { t.ID, t.name, t.createAt, t.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Tags.ToPagedListAsync(
                    t => new { t.ID, t.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<TagInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(t => ToTagInfo(t)).ToList()
            };
        }

        public PagedList<TagInfo> GetTags(int pageIndex, int pageSize)
        {
            SqlPagedList<Tag> pagedList = null;
            Expression<Func<Tag, object>> orderBy = t => new { t.ID };
            if (includeDescription && includeTimestamp)
                pagedList = db.Tags.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = db.Tags.ToPagedList(
                    t => new { t.ID, t.name, t.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeTimestamp)
                pagedList = db.Tags.ToPagedList(
                    t => new { t.ID, t.name, t.createAt, t.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Tags.ToPagedList(
                    t => new { t.ID, t.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<TagInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(t => ToTagInfo(t)).ToList()
            };
        }

        public async Task<TagInfo> GetTagAsync(long tagId)
        {
            if (tagId <= 0)
                throw new Exception("@'tagId' must be greater than 0");

            Tag tag = null;
            if (includeDescription && includeTimestamp)
                tag = await db.Tags.SingleOrDefaultAsync(t => t.ID == tagId);
            else if(includeDescription)
                tag = await db.Tags
                    .SingleOrDefaultAsync(t => new { t.ID, t.name, t.description }, t => t.ID == tagId);
            else if(includeTimestamp)
                tag = await db.Tags
                    .SingleOrDefaultAsync(t => new { t.ID, t.name, t.createAt, t.updateAt }, t => t.ID == tagId);
            else
                tag = await db.Tags.SingleOrDefaultAsync(t => new { t.ID, t.name }, t => t.ID == tagId);

            return ToTagInfo(tag);
        }

        public TagInfo GetTag(long tagId)
        {
            if (tagId <= 0)
                throw new Exception("@'tagId' must be greater than 0");

            Tag tag = null;
            if (includeDescription && includeTimestamp)
                tag = db.Tags.SingleOrDefault(t => t.ID == tagId);
            else if (includeDescription)
                tag = db.Tags
                    .SingleOrDefault(t => new { t.ID, t.name, t.description }, t => t.ID == tagId);
            else if (includeTimestamp)
                tag = db.Tags
                    .SingleOrDefault(t => new { t.ID, t.name, t.createAt, t.updateAt }, t => t.ID == tagId);
            else
                tag = db.Tags.SingleOrDefault(t => new { t.ID, t.name }, t => t.ID == tagId);

            return ToTagInfo(tag);
        }

        public async Task<List<TagInfo>> GetTagsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText;
            if (includeDescription && includeTimestamp)
                commandText = @"Select [Tag].* from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else if(includeDescription)
                commandText = @"Select [Tag].[ID], [Tag].[name], [Tag].[description] 
                                from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else if(includeTimestamp)
                commandText = @"Select [Tag].[ID], [Tag].[name], [Tag].[createAt], [Tag].[updateAt] 
                                from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else
                commandText = @"Select [Tag].[ID], [Tag].[name] 
                                from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";

            return await db.Execute_ToListAsync<TagInfo>(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
        }

        public List<TagInfo> GetTagsByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("@'filmId' must not be null or empty");

            string commandText;
            if (includeDescription && includeTimestamp)
                commandText = @"Select [Tag].* from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else if (includeDescription)
                commandText = @"Select [Tag].[ID], [Tag].[name], [Tag].[description] 
                                from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else if (includeTimestamp)
                commandText = @"Select [Tag].[ID], [Tag].[name], [Tag].[createAt], [Tag].[updateAt] 
                                from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else
                commandText = @"Select [Tag].[ID], [Tag].[name] 
                                from [TagDistribution], [Tag]
                                where [TagDistribution].[tagId] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";

            return db.Execute_ToList<TagInfo>(commandText, CommandType.Text, new SqlParameter("@filmId", filmId));
        }

        public async Task<CreationState> CreateTagAsync(TagCreation tagCreation)
        {
            Tag tag = ToTag(tagCreation);
            if (tag.name == null)
                throw new Exception("@'tag.name' must not be null");

            int checkExists = (int)await db.Tags.CountAsync(t => t.name == tag.name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (tag.description == null)
                affected = await db.Tags.InsertAsync(tag, new List<string> { "ID", "description" });
            else
                affected = await db.Tags.InsertAsync(tag, new List<string> { "ID" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateTagAsync(TagUpdate tagUpdate)
        {
            Tag tag = ToTag(tagUpdate);
            if (tag.name == null)
                throw new Exception("@'tag.name' must not be null");

            int affected;
            if (tag.description == null)
                affected = await db.Tags.UpdateAsync(
                    tag,
                    t => new { t.name, t.updateAt },
                    t => t.ID == tag.ID
                );
            else
                affected = await db.Tags.UpdateAsync(
                    tag,
                    t => new { t.name, t.description, t.updateAt },
                    t => t.ID == tag.ID
                );

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteTagAsync(long tagId)
        {
            if (tagId <= 0)
                throw new Exception("@'tagId' must be greater than 0");

            long tagDistributionNumber = await db.TagDistributions
                .CountAsync(td => td.tagId == tagId);
            if (tagDistributionNumber > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.Tags.DeleteAsync(t => t.ID == tagId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Tags.CountAsync();
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
