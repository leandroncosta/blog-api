import 'dart:math';
import 'package:flutter/material.dart';
import 'package:flutter_application/Providers/post_provider.dart';
import 'package:flutter_application/components/bottom_navigation_bar.dart';
import 'package:flutter_application/components/edit_post_modal.dart';
import 'package:flutter_application/components/form_field.dart';
import 'package:flutter_application/shared/controllers/post_controller.dart';
import 'package:flutter_application/shared/models/post_model.dart';
import 'package:intl/date_symbol_data_file.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class PostView extends StatefulWidget {
  const PostView({super.key});

  @override
  State<PostView> createState() => _MyWidgetState();
}

class _MyWidgetState extends State<PostView> {
  late Post post;
  final _formKey = GlobalKey<FormState>();
  PostController postController = PostController();
  final titleEditingController = TextEditingController();
  final contentEdidingController = TextEditingController();
  final idEditingController = TextEditingController();
  @override
  void initState() {
    super.initState();
    postController.start();
  }

  changeState(EPostState state) {
    switch (state) {
      case EPostState.start:
        return _start();
      case EPostState.loading:
        return _loading();
      case EPostState.success:
        return _success();
      case EPostState.error:
        return _error();
    }
  }

  _start() {
    return Container();
  }

  _error() {
    return Center(
        child: Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        const Text("Erro ao buscar os dados , tente novamente outra vez"),
        ElevatedButton(
            onPressed: () {
              postController.start();
            },
            child: const Text("Tentar novamente"))
      ],
    ));
  }

  _showEditModal(BuildContext context, String id) async {
    TextEditingController titleEditController = TextEditingController();
    TextEditingController contentEditController = TextEditingController();
    return showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          content: FutureBuilder(
            future: postController.findById(id),
            builder: (context, snapshot) {
              if (snapshot.hasData) {
                post = snapshot.data!;
                titleEditController.text = post.title;
                contentEditController.text = post.content;
                return SizedBox(
                  height: 150,
                  child: Form(
                      child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      FormFieldInput(
                          controller: titleEditController, labelText: 'título'),
                      const SizedBox(
                        height: 20,
                      ),
                      FormFieldInput(
                          controller: contentEditController,
                          labelText: 'conteúdo')
                    ],
                  )),
                );
              } else if (snapshot.hasError) {
                return Center(
                  child: Column(
                    children: [
                      Text("Houve um erro ao buscar o post ${snapshot.error}"),
                      Text(
                          style: const TextStyle(fontSize: 16),
                          snapshot.error.toString()),
                    ],
                  ),
                );
              } else if (snapshot.connectionState == ConnectionState.waiting) {
                return const Column(children: [
                  CircularProgressIndicator(),
                  Text("Carregando")
                ]);
              }
              return const Center(
                  child: Column(
                children: [Text("Carregando os dados... aguarde")],
              ));
            },
          ),
          actions: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                ElevatedButton(
                  onPressed: () {
                    Navigator.pop(context);
                  },
                  child: const Text("Cancelar"),
                ),
                ElevatedButton(
                  onPressed: () {
                    Post updatedPost = Post(
                      id: post.id,
                      userId: "",
                      title: titleEditController.text,
                      content: contentEditController.text,
                      date: "",
                    );
                    postController.update(updatedPost);

                    Navigator.of(context).pop();
                  },
                  child: const Text("Salvar"),
                ),
              ],
            )
          ],
        );
      },
    );
  }

  _success() {
    if (postController.posts.isEmpty) {
      return const Center(
        child: Text("Nenhum post disponível"),
      );
    }
    return ListView.builder(
        padding: const EdgeInsets.only(top: 5, bottom: 70),
        itemCount: postController.posts.length,
        itemBuilder: (context, index) {
          var p = postController.posts[index];
          var date = DateTime.parse(p.date);
          var utc3Date = date.subtract(const Duration(hours: 3));
          var formatedDate = DateFormat("dd/MM/yyyy").add_jm().format(utc3Date);
          return Padding(
            padding: const EdgeInsets.only(bottom: 4),
            child: Container(
              decoration: BoxDecoration(
                  border: Border.all(
                      color: const Color.fromARGB(255, 212, 215, 212),
                      width: 3)),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  SizedBox(
                    height: 200,
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(
                          "Post id: ${p.id}",
                        ),
                        const SizedBox(
                          height: 20,
                        ),
                        Text(
                          "Post ${p.userId}",
                          style: const TextStyle(fontSize: 20),
                        ),
                        Text(
                          "Título - ${p.title}",
                          style: const TextStyle(fontSize: 20),
                        ),
                        Text(
                          "Conteúdo - ${p.content}",
                          style: const TextStyle(fontSize: 20),
                        ),
                        Text(
                          "Data de criação : ${formatedDate}",
                          style: const TextStyle(fontSize: 20),
                        ),
                        const SizedBox(
                          height: 10,
                        ),
                      ],
                    ),
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: <Widget>[
                      ElevatedButton(
                        child: const Text('editar'),
                        onPressed: () {
                          _showEditModal(context, p.id);
                        },
                      ),
                      const SizedBox(width: 200),
                      ElevatedButton(
                        child: const Text('deletar'),
                        onPressed: () {
                          _showDeleteModal(context, p.id);
                        },
                      ),
                    ],
                  ),
                ],
              ),
            ),
          );
        });
  }

  _loading() {
    return const Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [CircularProgressIndicator(), Text("Carregando os dados")],
      ),
    );
  }

  _createpost() {
    post = Post(
        id: "",
        userId: "",
        title: titleEditingController.text,
        content: contentEdidingController.text,
        date: "");
    postController.createPost(post);
    //_futurePosts=postController.findAll();
    titleEditingController.clear();
    contentEdidingController.clear();
  }

  _showAddPostDialog() {
    return showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
            title: const Center(child: Text("Adicionar post")),
            content: SizedBox(
              height: 200,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Form(
                      key: _formKey,
                      child: Column(
                        children: <Widget>[
                          TextFormField(
                            validator: (value) {
                              if (value == null || value.isEmpty) {
                                return "Por favor insira um título";
                              }
                              return null;
                            },
                            decoration:
                                const InputDecoration(labelText: "Título"),
                            controller: titleEditingController,
                          ),
                          TextFormField(
                            validator: (value) {
                              if (value == null || value.isEmpty) {
                                return "Por favor, insira o conteúdo do post";
                              }
                              return null;
                            },
                            decoration:
                                const InputDecoration(labelText: "Conteúdo"),
                            controller: contentEdidingController,
                          ),
                        ],
                      ))
                ],
              ),
            ),
            actions: [
              ElevatedButton(
                onPressed: () {
                  if (_formKey.currentState!.validate()) {
                    ScaffoldMessenger.of(context).showSnackBar(
                        const SnackBar(content: Text('Processando dados')));
                    _createpost();
                  }
                },
                child: const Text("criar post"),
              ),
              ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text("cancelar"),
              ),
            ]);
      },
    );
  }

  _showDeleteModal(context, id) {
    return showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
            title: Text(
              style: const TextStyle(color: Colors.red),
              textAlign: TextAlign.center,
              "Tem certeza que deseja deletar o  post de id :$id ?",
            ),
            content: const SizedBox(
              height: 50,
            ),
            actions: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text("cancelar"),
              ),
              ElevatedButton(
                onPressed: () {
                  postController.delete(id);
                  Navigator.of(context).pop();
                },
                child: const Text("deletar post"),
              ),
            ]);
      },
    );
  }

  _showFindPostDialog() {
    TextEditingController idController = TextEditingController();
    return showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          content: FormFieldInput(
              controller: idController, labelText: "Digite o id do post"),
          actions: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                ElevatedButton(
                  onPressed: () {
                    Navigator.pop(context);
                  },
                  child: const Text("Cancelar"),
                ),
                ElevatedButton(
                  onPressed: () {
                    Navigator.pop(context);
                    _showEditModal(context, idController.text);
                  },
                  child: const Text("Buscar"),
                ),
              ],
            )
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    const WidgetStateProperty<Icon> thumbIcon =
        WidgetStateProperty<Icon>.fromMap(
      <WidgetStatesConstraint, Icon>{
        WidgetState.selected: Icon(Icons.dark_mode),
        WidgetState.any: Icon(Icons.light_mode),
      },
    );
    return Scaffold(
      appBar: AppBar(
        title: Container(
          padding: const EdgeInsets.only(right: 50),
          child:
              const Center(widthFactor: 200, child: Text("Listagem de Posts")),
        ),
        leading: Switch(
            thumbIcon: thumbIcon,
            value: context.read<PostProvider>().isDarktheme,
            onChanged: (bool? value) {
              setState(() {
                context.read<PostProvider>().changeTheme();
              });
            }),
      ),
      body: AnimatedBuilder(
          animation: postController.state,
          builder: (context, child) {
            return changeState(postController.state.value);
          }),
      floatingActionButton: Padding(
        padding: const EdgeInsets.only(left: 30),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            FloatingActionButton(
                child: const Icon(Icons.search),
                onPressed: () {
                  _showFindPostDialog();
                }),
            FloatingActionButton(
                child: const Icon(Icons.add),
                onPressed: () {
                  _showAddPostDialog();
                }),
          ],
        ),
      ),
      bottomNavigationBar: BottomNavBar(),
    );
  }
}
