namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CACHORRO")]
    public partial class CACHORRO
    {
        [Key]
        [Column(Order = 0)]
        public string name { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int database_id { get; set; }

        public int? source_database_id { get; set; }

        [MaxLength(85)]
        public byte[] owner_sid { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime create_date { get; set; }

        [Key]
        [Column(Order = 3)]
        public byte compatibility_level { get; set; }

        [StringLength(128)]
        public string collation_name { get; set; }

        public byte? user_access { get; set; }

        [StringLength(60)]
        public string user_access_desc { get; set; }

        public bool? is_read_only { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool is_auto_close_on { get; set; }

        public bool? is_auto_shrink_on { get; set; }

        public byte? state { get; set; }

        [StringLength(60)]
        public string state_desc { get; set; }

        public bool? is_in_standby { get; set; }

        public bool? is_cleanly_shutdown { get; set; }

        public bool? is_supplemental_logging_enabled { get; set; }

        public byte? snapshot_isolation_state { get; set; }

        [StringLength(60)]
        public string snapshot_isolation_state_desc { get; set; }

        public bool? is_read_committed_snapshot_on { get; set; }

        public byte? recovery_model { get; set; }

        [StringLength(60)]
        public string recovery_model_desc { get; set; }

        public byte? page_verify_option { get; set; }

        [StringLength(60)]
        public string page_verify_option_desc { get; set; }

        public bool? is_auto_create_stats_on { get; set; }

        public bool? is_auto_create_stats_incremental_on { get; set; }

        public bool? is_auto_update_stats_on { get; set; }

        public bool? is_auto_update_stats_async_on { get; set; }

        public bool? is_ansi_null_default_on { get; set; }

        public bool? is_ansi_nulls_on { get; set; }

        public bool? is_ansi_padding_on { get; set; }

        public bool? is_ansi_warnings_on { get; set; }

        public bool? is_arithabort_on { get; set; }

        public bool? is_concat_null_yields_null_on { get; set; }

        public bool? is_numeric_roundabort_on { get; set; }

        public bool? is_quoted_identifier_on { get; set; }

        public bool? is_recursive_triggers_on { get; set; }

        public bool? is_cursor_close_on_commit_on { get; set; }

        public bool? is_local_cursor_default { get; set; }

        public bool? is_fulltext_enabled { get; set; }

        public bool? is_trustworthy_on { get; set; }

        public bool? is_db_chaining_on { get; set; }

        public bool? is_parameterization_forced { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool is_master_key_encrypted_by_server { get; set; }

        public bool? is_query_store_on { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool is_published { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool is_subscribed { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool is_merge_published { get; set; }

        [Key]
        [Column(Order = 9)]
        public bool is_distributor { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool is_sync_with_backup { get; set; }

        [Key]
        [Column(Order = 11)]
        public Guid service_broker_guid { get; set; }

        [Key]
        [Column(Order = 12)]
        public bool is_broker_enabled { get; set; }

        public byte? log_reuse_wait { get; set; }

        [StringLength(60)]
        public string log_reuse_wait_desc { get; set; }

        [Key]
        [Column(Order = 13)]
        public bool is_date_correlation_on { get; set; }

        [Key]
        [Column(Order = 14)]
        public bool is_cdc_enabled { get; set; }

        public bool? is_encrypted { get; set; }

        public bool? is_honor_broker_priority_on { get; set; }

        public Guid? replica_id { get; set; }

        public Guid? group_database_id { get; set; }

        public int? resource_pool_id { get; set; }

        public short? default_language_lcid { get; set; }

        [StringLength(128)]
        public string default_language_name { get; set; }

        public int? default_fulltext_language_lcid { get; set; }

        [StringLength(128)]
        public string default_fulltext_language_name { get; set; }

        public bool? is_nested_triggers_on { get; set; }

        public bool? is_transform_noise_words_on { get; set; }

        public short? two_digit_year_cutoff { get; set; }

        public byte? containment { get; set; }

        [StringLength(60)]
        public string containment_desc { get; set; }

        public int? target_recovery_time_in_seconds { get; set; }

        public int? delayed_durability { get; set; }

        [StringLength(60)]
        public string delayed_durability_desc { get; set; }

        public bool? is_memory_optimized_elevate_to_snapshot_on { get; set; }
    }
}
