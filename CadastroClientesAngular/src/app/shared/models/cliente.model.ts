
export interface Endereco {
    cep: string;
    logradouro: string;
    numero: string;
    bairro: string;
    cidade: string;
    estado: string;
  }
  
  export interface Cliente {
    id: string;
    nome: string;
    documento: string;
    tipoPessoa: string;
    dataNascimento: Date;
    telefone: string;
    email: string;
    inscricaoEstadual?: string;
    isento: boolean;
    endereco: Endereco;
  }