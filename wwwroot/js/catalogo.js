$(document).ready(function () {
  let cart = {};

  // Função para buscar produtos do JSON
  function loadProducts() {
    $.getJSON("../../db/produtos.json", function (products) {
      displayProducts(products);
    });
  }

  function displayProducts(products) {
    const productList = $("#productList");
    productList.empty(); // Limpa a lista de produtos
    products.forEach((product) => {
      const card = `
              <div class="col-md-4">
                  <div class="card product-card">
                      <img src="${
                        product.ImagemUrl
                      }.jpg" class="card-img-top" alt="${product.Nome}" />
                      <div class="card-body">
                          <h5 class="card-title">${product.Nome}</h5>
                          <p class="card-text">${product.Descricao}</p>
                          <p class="card-text">Preço: R$ ${product.Preco.toFixed(
                            2
                          )}</p>
                          <div class="quantity-controls">
                              <button onclick="addToCart(${
                                product.Id
                              })">Adicionar ao Carrinho</button>
                          </div>
                      </div>
                  </div>
              </div>
          `;
      productList.append(card);
    });
  }

  function addToCart(productId) {
    // Lógica de adição ao carrinho...
  }

  function updateCart() {
    // Lógica de atualização do carrinho...
  }

  function removeFromCart(productId) {
    // Lógica de remoção do carrinho...
  }

  $("#search").on("input", function () {
    // Lógica para filtragem de produtos...
  });

  $("#category").on("change", function () {
    // Lógica para filtragem de categorias...
  });

  $("#sort").on("change", function () {
    // Lógica para ordenação de produtos...
  });

  // Carregar produtos ao iniciar
  loadProducts();
});
