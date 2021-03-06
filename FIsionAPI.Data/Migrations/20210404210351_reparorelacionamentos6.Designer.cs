// <auto-generated />
using System;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FIsionAPI.Data.Migrations
{
    [DbContext(typeof(FisionContext))]
    [Migration("20210404210351_reparorelacionamentos6")]
    partial class reparorelacionamentos6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntidadeEspecialidades", b =>
                {
                    b.Property<Guid>("EntidadesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EspecialidadesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EntidadesId", "EspecialidadesId");

                    b.HasIndex("EspecialidadesId");

                    b.ToTable("EntidadeEspecialidades");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Caixa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Competencia")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("ValorDespesa")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorReceita")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Caixa");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.ContratoFinanceiro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Desconto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("EntidadeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("MargemLucro")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorMensal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Vencimento")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("EntidadeId")
                        .IsUnique();

                    b.ToTable("ContratoFinanceiro");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.EnderecoPessoa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bairro")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Complemento")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("PessoaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PessoaId")
                        .IsUnique();

                    b.ToTable("EnderecoPessoa");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Entidade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Classe")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtEntrada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtSaida")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EspecialidadeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Matricula")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("PessoaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PessoaId");

                    b.ToTable("Entidades");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Especialidades", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Especialidades");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.MovimentoFinanceiro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CaixaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Classe")
                        .HasColumnType("int");

                    b.Property<string>("Competencia")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("ContratoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtPagamento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtVencimento")
                        .HasColumnType("datetime2");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorEmAberto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorPago")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CaixaId");

                    b.HasIndex("ContratoId");

                    b.ToTable("MovimentoFinanceiro");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Pessoa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<int>("Classe")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nome")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Sexo")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Telefone")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("EntidadeEspecialidades", b =>
                {
                    b.HasOne("FIsionAPI.Business.Models.Entidade", null)
                        .WithMany()
                        .HasForeignKey("EntidadesId")
                        .IsRequired();

                    b.HasOne("FIsionAPI.Business.Models.Especialidades", null)
                        .WithMany()
                        .HasForeignKey("EspecialidadesId")
                        .IsRequired();
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.ContratoFinanceiro", b =>
                {
                    b.HasOne("FIsionAPI.Business.Models.Entidade", "Entidade")
                        .WithOne("Contrato")
                        .HasForeignKey("FIsionAPI.Business.Models.ContratoFinanceiro", "EntidadeId")
                        .IsRequired();

                    b.Navigation("Entidade");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.EnderecoPessoa", b =>
                {
                    b.HasOne("FIsionAPI.Business.Models.Pessoa", "Pessoa")
                        .WithOne("Endereco")
                        .HasForeignKey("FIsionAPI.Business.Models.EnderecoPessoa", "PessoaId")
                        .IsRequired();

                    b.Navigation("Pessoa");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Entidade", b =>
                {
                    b.HasOne("FIsionAPI.Business.Models.Pessoa", "Pessoa")
                        .WithMany("Entidades")
                        .HasForeignKey("PessoaId")
                        .IsRequired();

                    b.Navigation("Pessoa");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.MovimentoFinanceiro", b =>
                {
                    b.HasOne("FIsionAPI.Business.Models.Caixa", "Caixa")
                        .WithMany("Movimentos")
                        .HasForeignKey("CaixaId");

                    b.HasOne("FIsionAPI.Business.Models.ContratoFinanceiro", "Contrato")
                        .WithMany("Movimentos")
                        .HasForeignKey("ContratoId")
                        .IsRequired();

                    b.Navigation("Caixa");

                    b.Navigation("Contrato");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Caixa", b =>
                {
                    b.Navigation("Movimentos");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.ContratoFinanceiro", b =>
                {
                    b.Navigation("Movimentos");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Entidade", b =>
                {
                    b.Navigation("Contrato");
                });

            modelBuilder.Entity("FIsionAPI.Business.Models.Pessoa", b =>
                {
                    b.Navigation("Endereco");

                    b.Navigation("Entidades");
                });
#pragma warning restore 612, 618
        }
    }
}
