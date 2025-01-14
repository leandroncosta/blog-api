import 'dart:math';

import 'package:flutter/material.dart';
import 'package:flutter_application/Providers/post_provider.dart';
import 'package:flutter_application/components/bottom_navigation_bar.dart';
import 'package:flutter_application/components/form_field.dart';
import 'package:flutter_application/pages/posts/list_posts.dart';
import 'package:flutter_application/shared/controllers/post_controller.dart';
import 'package:flutter_application/shared/models/post_model.dart';
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
                  child: Text(
                      style: const TextStyle(fontSize: 16),
                      snapshot.error.toString()),
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
                    //postController.update(updatedPost);
                    context.read<PostProvider>().update(updatedPost);
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

  _createpost() {
    post = Post(
        id: "",
        userId: "",
        title: titleEditingController.text,
        content: contentEdidingController.text,
        date: "");
    context.read<PostProvider>().createPost(post);
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
                        const SnackBar(
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.only(topLeft: Radius.elliptical(50, 100),topRight: Radius.circular(70))),
                          backgroundColor: Colors.amber,
                          content: Text('Processando dados')));
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
  void initState() {
    super.initState();
    //context.read<PostController>().start();
    context.read<PostProvider>();
  }

  //@override
  // void dispose() {
  //   super.dispose();
  //   context.read<PostProvider>();
  // }

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
            //activeColor: Color(value),
            thumbIcon: thumbIcon,
            value: context.read<PostProvider>().isDarktheme,
            onChanged: (bool? value) {
              setState(() {
                context.read<PostProvider>().changeTheme();
              });
            }),
      ),
      body: FutureBuilder(
        future: context.watch<PostProvider>().futurePosts,
        //future: context.watch<PostController>().futurePosts,
        builder: (context, snapshot) {
          if (snapshot.hasData) {
            final posts = snapshot.data!;
            return ListPosts(posts: posts);
          } else if (snapshot.hasError) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                      textAlign: TextAlign.center,
                      style: const TextStyle(fontSize: 16, color: Colors.red),
                      snapshot.error.toString()),
                ],
              ),
            );
          } else if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(
              child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [CircularProgressIndicator(), Text("Carregando")]),
            );
          }
          return const Scaffold();
        },
      ),
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
      bottomNavigationBar: const BottomNavBar(),
    );
  }
}
